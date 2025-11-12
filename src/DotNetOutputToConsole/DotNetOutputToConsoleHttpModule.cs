

using System.Web;

namespace DotNetOutputToConsole
{
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

        public void Dispose() { }
    }
}
