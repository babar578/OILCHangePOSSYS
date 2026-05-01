using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;
using POS.Web.Controllers;

namespace POS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configure Hangfire - Use ControlDB for storing Hangfire jobs
            string connectionString = ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;
            
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1)
                });

            // Note: Background jobs will be configured per-tenant
            // For now, commented out the global SMS job
            // See Phase 8 of multi-tenant implementation plan for tenant-aware background jobs
            
            // TODO: Implement tenant-aware background jobs
            // Example: Schedule jobs for all active tenants
            // var tenants = GetAllActiveTenants();
            // foreach (var tenant in tenants)
            // {
            //     RecurringJob.AddOrUpdate(
            //         $"oil-change-reminder-sms-{tenant.TenantId}",
            //         () => HomeController.SendOilChangeReminderSMS(tenant.TenantId),
            //         Cron.Daily(9, 0)
            //     );
            // }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Increased session timeout to 1440 minutes (24 hours) to prevent frequent session expiration
            Session.Timeout = 1440;
        }

    }
}
