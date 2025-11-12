# DotNetOutputToConsole

> 🧩 A secure ASP.NET Framework (4.8 / 4.8.1) helper library that writes logs, variables, and exceptions directly to the browser console — perfect for UAT and DEV environments.

---

## ✨ Features
- ✅ Write messages, variables, and errors to the browser console (`console.info`, `console.log`, `console.error`)
- ✅ Automatically logs **unhandled exceptions** globally
- ✅ Works with any **ASP.NET Web Forms** or **MVC** project
- ✅ Simple global toggle via `web.config`
- ✅ **XSS-safe** — uses `System.Web.Helpers.Json.Encode` to sanitize all outputs
- ✅ Zero third-party dependencies
- ✅ Includes **unit tests** and a **demo web app**
- ✅ Fully compatible with **.NET Framework 4.8 → 4.8.1**

---

## ⚙️ Installation
Install from NuGet using the .NET CLI or Visual Studio Package Manager Console:

```powershell
Install-Package DotNetOutputToConsole
```

---

## 💡 Purpose
When deploying ASP.NET apps to UAT or DEV servers, developers often need to see **runtime values** or **exceptions** directly in the browser console.  
This library provides a **safe, fast, and zero-config** way to do that, without changing your page layout or using intrusive alert popups.

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

---

## 🧠 Architecture Overview
| Component | Description |
|------------|--------------|
| `DotNetOutputToConsoleLogger` | Core class that writes safe `<script>` tags to the HTTP response with sanitized console commands. |
| `DotNetOutputToConsoleHttpModule` | Global ASP.NET module that automatically logs **unhandled exceptions** to the console. |
| `web.config` switch | Toggle feature ON/OFF without recompiling. |
| `Json.Encode()` | Ensures all messages are XSS-safe before rendering. |

---

## 📂 Configuration
Add this to your **Web.config**:
```xml
<configuration>
  <appSettings>
    <add key="EnableOutputToConsole" value="true" />
  </appSettings>

  <system.webServer>
    <modules>
      <add name="DotNetOutputToConsoleHttpModule" type="DotNetOutputToConsole.DotNetOutputToConsoleHttpModule" />
    </modules>
  </system.webServer>
</configuration>
```

✅ Set `"EnableOutputToConsole"` to `"true"` in UAT/DEV.  
🚫 Set to `"false"` in production for performance and security.

---

## 🧩 Usage Examples
### 1️⃣ Log Information
```csharp
DotNetOutputToConsoleLogger.LogInfo("Page load completed");
```
Console output:
```
INFO: Page load completed
```

### 2️⃣ Log Variables
```csharp
DotNetOutputToConsoleLogger.LogVariable("Username", user.Name);
```
Console output:
```
LOG: Username: JohnDoe
```

### 3️⃣ Log Exceptions
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
Console output:
```
ERROR: Simulated failure
```

### 4️⃣ Automatic Error Logging (No Try/Catch Needed)
The built-in HTTP module automatically logs unhandled exceptions:
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    throw new Exception("Unhandled page error!");
}
```
Console output:
```
ERROR: Unhandled Exception: Unhandled page error!
```

---

## 🧩 Class Overview
### 🔹 DotNetOutputToConsoleLogger.cs
```csharp
public static class DotNetOutputToConsoleLogger
{
    public static void LogInfo(string message);
    public static void LogVariable(string name, object value);
    public static void LogError(string message);
}
```
All three use `Json.Encode()` internally to prevent XSS or script injection.

### 🔹 DotNetOutputToConsoleHttpModule.cs
Automatically hooks into the ASP.NET pipeline:
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

---

## 🔒 Security
| Protection | Description |
|-------------|--------------|
| **XSS Safe** | Uses `System.Web.Helpers.Json.Encode()` to escape any special characters before writing scripts. |
| **Response-Safe** | Only writes inside valid `<script>` tags. |
| **Config Toggle** | Easily disable all console output via Web.config. |
| **Recommended Use** | Only enable in DEV or UAT environments. |

---

## 🧪 Unit Testing
### Installation
```powershell
Install-Package NUnit
Install-Package NUnit3TestAdapter
Install-Package Microsoft.NET.Test.Sdk
```
### Example Test
```csharp
using NUnit.Framework;
using DotNetOutputToConsole;

namespace DotNetOutputToConsole.Tests
{
    [TestFixture]
    public class DotNetOutputToConsoleLoggerTests
    {
        [Test]
        public void LogInfo_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogInfo("info"));
        [Test]
        public void LogError_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogError("error"));
        [Test]
        public void LogVariable_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogVariable("Key", "Value"));
    }
}
```
### Run Tests
In Visual Studio 2022 → **Test → Test Explorer → Run All Tests**  
or CLI:
```bash
dotnet test tests/DotNetOutputToConsole.Tests/DotNetOutputToConsole.Tests.csproj
```

---

## 🧱 Project Structure
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

---

## 🧩 How It Works
1. `DotNetOutputToConsoleLogger` calls `HttpContext.Current.Response.Write(...)`
2. It injects a `<script>` tag that runs a console command like:
   ```html
   <script>console.log("Your message");</script>
   ```
3. All messages are escaped via `Json.Encode()`.
4. View messages in browser **Developer Tools → Console tab**.

---

## 🧑‍💻 Author
**Created by:** `livedcode`  
GitHub: [https://github.com/livedcode](https://github.com/livedcode)  
NuGet: [https://www.nuget.org/profiles/livedcode](https://www.nuget.org/profiles/livedcode)

---

## 📜 License
MIT License © 2025 livedcode

---

## 🧭 Future Enhancements
- Add `LogWarning()` → `console.warn()`
- Dual logging (console + file)
- ASP.NET Core middleware support
- MVC exception filter integration

---

> 💡 “The simplest and safest way to see your ASP.NET logs in the browser console.”
