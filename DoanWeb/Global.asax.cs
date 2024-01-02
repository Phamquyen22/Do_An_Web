using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoanWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            // Log information or perform actions related to session start
            string sessionId = Session.SessionID;
            string userName = HttpContext.Current.User.Identity.Name;

            // Example: Logging session start information
            System.Diagnostics.Debug.WriteLine($"Session Start - Session ID: {sessionId}, User: {userName}");
        }

        // This method is called when an existing session ends
        protected void Session_End(object sender, EventArgs e)
        {
            // Log information or perform actions related to session end
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string sessionId = Session.SessionID;
                string userName = HttpContext.Current.User.Identity.Name;

                // Tiếp tục xử lý
                System.Diagnostics.Debug.WriteLine($"Session End - Session ID: {sessionId}, User: {userName}");
            }
            
        }
    }
}
