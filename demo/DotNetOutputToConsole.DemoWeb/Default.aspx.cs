using System;
using DotNetOutputToConsole; 

namespace DotNetOutputToConsole.DemoWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            DotNetOutputToConsoleLogger.LogInfo("Button clicked");
            DotNetOutputToConsoleLogger.LogVariable("CurrentUser", Environment.UserName);
            try { 
                throw new InvalidOperationException("Sample exception thwrow"); 
            }
            catch (Exception ex) 
            { 
                DotNetOutputToConsoleLogger.LogError(ex.Message); 
            }
        }
    }
}