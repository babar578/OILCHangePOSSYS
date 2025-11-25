using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(POS.Web.Startup))]

namespace POS.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Hangfire Dashboard
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            // Start Hangfire Server
            app.UseHangfireServer();
        }
    }

    // Simple authorization filter - you can customize this based on your authentication needs
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // For now, allow all access. You can add authentication logic here
            // Example: Check if user is authenticated and has admin role
            // To access HTTP context in the future, you can use:
            // var owinEnvironment = context.GetOwinEnvironment();
            // var httpContext = HttpContext.Current; // For System.Web
            return true; // Change this to implement proper authorization
        }
    }
}

