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

            // Configure Hangfire
            string connectionString = ConfigurationManager.ConnectionStrings["Dock27PosWebPortalConnectionString"].ConnectionString;
            
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

            // Schedule daily SMS reminder job - runs once per day at 9:00 AM
            RecurringJob.AddOrUpdate(
                "oil-change-reminder-sms",
                () => HomeController.SendOilChangeReminderSMS(),
                Cron.Daily(9, 0),
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Local
                });
        }

        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 900;
        }

    }
}
