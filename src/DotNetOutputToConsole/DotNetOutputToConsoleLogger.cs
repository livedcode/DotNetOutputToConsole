using System;
using System.Web;
using System.Configuration;
using System.Web.Helpers;

namespace DotNetOutputToConsole
{
    /// <summary>
    /// Securely writes diagnostic messages and variables to the browser console.
    /// Uses Json.Encode() to avoid XSS. Toggle via EnableOutputToConsole in web.config.
    /// </summary>
    public static class DotNetOutputToConsoleLogger
    {

        private static bool IsEnabled =>
            string.Equals(ConfigurationManager.AppSettings["EnableOutputToConsole"], "true", StringComparison.OrdinalIgnoreCase);

        public static void LogInfo(string message)
        {
            if (!IsEnabled) return;
            WriteScript("info", message);
        }

        public static void LogError(string message)
        {
            if (!IsEnabled) return;
            WriteScript("error", message);
        }

        public static void LogVariable(string name, object value)
        {
            if (!IsEnabled) return;
            string combined = $"{name}: {value}";
            WriteScript("log", combined);
        }

        private static void WriteScript(string level, string message)
        {
            var context = HttpContext.Current;
            if (context?.Response == null) return;

            var encoded = Json.Encode($"{level.ToUpper()}: {message}");
            context.Response.Write($"<script>console.{level}({encoded});</script>");
        }
    }
}    

