using POS.Utilities.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace POS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //Seeder.Seed();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // API route for leads
            routes.MapRoute(
                name: "LeadsAPI",
                url: "api/leads",
                defaults: new { controller = "Website", action = "SubmitLeadPublic" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
