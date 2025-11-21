using System.Configuration;
using System.Web;

namespace DotNetOutputToConsole
{
    public class DotNetOutputToConsoleHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (s, ev) =>
            {
                string enabled = ConfigurationManager.AppSettings["EnableOutputToConsole"] ?? "false";
                HttpContext.Current.Application["EnableOutputToConsole"] = enabled;
            };

            context.Error += (sender, e) =>
            {
                var ex = HttpContext.Current?.Server.GetLastError();
                if (ex != null)
                    DotNetOutputToConsoleLogger.LogError("Unhandled Exception: " + ex.Message);
            };
        }

        public void Dispose() { }
    }
}

