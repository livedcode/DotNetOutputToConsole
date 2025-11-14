# DotNetOutputToConsole

A secure ASP.NET Framework (4.8 / 4.8.1) helper library that writes logs, variables, and exceptions directly to the browser console — perfect for UAT and DEV environments.

## Features
- Write messages, variables, and errors to the browser console (`console.info`, `console.log`, `console.error`)
- Automatically logs unhandled exceptions globally
- Works with any ASP.NET Web Forms or MVC project
- Simple global toggle via `web.config`
- XSS-safe using `System.Web.Helpers.Json.Encode` to sanitize all outputs
- Zero third-party dependencies
- Includes unit tests and a demo web app
- Fully compatible with .NET Framework 4.8 to 4.8.1

## Installation
Install from NuGet using the Package Manager Console:

```powershell
Install-Package DotNetOutputToConsole
```

## Purpose
When deploying ASP.NET applications to UAT or DEV environments, developers often need to inspect runtime values or exceptions directly in the browser console.
This library provides a safe, fast, invisible way to do that without altering page UI or showing alert popups.

Example:

```csharp
DotNetOutputToConsoleLogger.LogVariable("SessionId", Session.SessionID);
DotNetOutputToConsoleLogger.LogError("Missing input data");
```

Console output:

```
LOG: SessionId: 1a2b3c4d
ERROR: Missing input data
```

## Architecture Overview

| Component | Description |
|----------|-------------|
| `DotNetOutputToConsoleLogger` | Core class that writes sanitized console commands into browser output. |
| `DotNetOutputToConsoleHttpModule` | Automatically logs unhandled exceptions at the application level. |
| `web.config` switch | Allows turning console output on or off globally. |
| `Json.Encode()` | Sanitizes all messages to avoid XSS or script injection. |

## Configuration

Add the following to your Web.config:

```xml
<configuration>
  <appSettings>
    <add key="EnableOutputToConsole" value="true" />
  </appSettings>

  <system.webServer>
    <modules>
      <add name="DotNetOutputToConsoleHttpModule"
           type="DotNetOutputToConsole.DotNetOutputToConsoleHttpModule" />
    </modules>
  </system.webServer>
</configuration>
```

Set `"EnableOutputToConsole"` to `"true"` in UAT or DEV.
Set to `"false"` in production.

## Usage Examples

### Log Information
```csharp
DotNetOutputToConsoleLogger.LogInfo("Page load completed");
```

Output:
```
INFO: Page load completed
```

### Log Variables
```csharp
DotNetOutputToConsoleLogger.LogVariable("Username", user.Name);
```

Output:
```
LOG: Username: JohnDoe
```

### Log Exceptions
```csharp
try
{
    throw new Exception("Simulated failure");
}
catch (Exception ex)
{
    DotNetOutputToConsoleLogger.LogError(ex.Message);
}
```

Output:
```
ERROR: Simulated failure
```

### Automatic Error Logging (Unhandled Exceptions)
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    throw new Exception("Unhandled page error!");
}
```

Output:
```
ERROR: Unhandled Exception: Unhandled page error!
```

## Class Overview

### DotNetOutputToConsoleLogger.cs
```csharp
public static class DotNetOutputToConsoleLogger
{
    public static void LogInfo(string message);
    public static void LogVariable(string name, object value);
    public static void LogError(string message);
}
```

### DotNetOutputToConsoleHttpModule.cs
```csharp
public class DotNetOutputToConsoleHttpModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.Error += (sender, e) =>
        {
            var ex = HttpContext.Current?.Server.GetLastError();
            if (ex != null)
                DotNetOutputToConsoleLogger.LogError($"Unhandled Exception: {ex.Message}");
        };
    }
}
```

## Security

| Feature | Description |
|--------|-------------|
| XSS Protection | All messages pass through `Json.Encode()` to escape special characters. |
| Safe Output | Only writes inside valid `<script>` blocks. |
| Config Toggle | Can be disabled instantly using Web.config. |
| Recommended Usage | Enable only in UAT/DEV. |

## Unit Testing

### Test Framework Installation
```powershell
Install-Package NUnit
Install-Package NUnit3TestAdapter
Install-Package Microsoft.NET.Test.Sdk
```

### Example Test
```csharp
[Test]
public void LogInfo_ShouldNotThrow()
{
    Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogInfo("info"));
}
```

### Run Tests
Visual Studio 2022 → Test Explorer → Run All Tests  
or CLI:

```bash
dotnet test
```

## Project Structure

```
DotNetOutputToConsole/
├── src/
│   └── DotNetOutputToConsole/
│       ├── DotNetOutputToConsoleLogger.cs
│       ├── DotNetOutputToConsoleHttpModule.cs
│       ├── Properties/
│       │   └── AssemblyInfo.cs
│       ├── README.md
│       └── LICENSE
├── demo/
│   └── DotNetOutputToConsole.DemoWeb/
│       ├── Default.aspx
│       ├── Default.aspx.cs
│       └── Web.config
└── tests/
    └── DotNetOutputToConsole.Tests/
        └── DotNetOutputToConsoleLoggerTests.cs
```

## How It Works
1. The logger injects `<script>` tags into the HTTP response.
2. Messages are encoded using `Json.Encode()` to prevent script injection.
3. Output executes inside the browser console.
4. View results using Developer Tools → Console.

## Author
Created by: **livedcode**  
GitHub: https://github.com/livedcode  
NuGet: https://www.nuget.org/profiles/livedcode

## License
MIT License © 2025 livedcode
