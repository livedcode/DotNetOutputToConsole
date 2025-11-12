using NUnit.Framework;
using DotNetOutputToConsole;

namespace DotNetOutputToConsole.Tests
{
    [TestFixture]
    public class DotNetOutputToConsoleLoggerTests
    {
        [Test] public void LogInfo_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogInfo("test info"));
        [Test] public void LogError_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogError("test error"));
        [Test] public void LogVariable_ShouldNotThrow() => Assert.DoesNotThrow(() => DotNetOutputToConsoleLogger.LogVariable("User", "livedcode"));
    }
}