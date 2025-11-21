using System;
using System.Web;

namespace DotNetOutputToConsole
{
    /// <summary>
    /// Securely writes diagnostic messages and variables to the browser console.
    /// Uses Json.Encode() to avoid XSS. Toggle via EnableOutputToConsole in web.config.
    /// </summary>
    public static class DotNetOutputToConsoleLogger
    {

        private static bool IsEnabled =>
             string.Equals(
                 HttpContext.Current?.Application["EnableOutputToConsole"]?.ToString(),
                 "true",
                 StringComparison.OrdinalIgnoreCase);

        private static string Escape(string input) =>
            HttpUtility.JavaScriptStringEncode(input ?? string.Empty);

        private static void WriteToResponse(string script)
        {
            var ctx = HttpContext.Current;
            if (ctx == null || !IsEnabled)
                return;

            ctx.Response.Write($"<script>{script}</script>");
        }

        public static void LogInfo(string message) =>
            WriteToResponse($"console.info(\"{Escape(message)}\");");

        public static void LogVariable(string name, object value)
        {
            string safeName = Escape(name);
            string safeValue = Escape(value?.ToString());
            WriteToResponse($"console.log(\"{safeName}: {safeValue}\");");
        }

        public static void LogError(string message) =>
            WriteToResponse($"console.error(\"{Escape(message)}\");");
    }
}


