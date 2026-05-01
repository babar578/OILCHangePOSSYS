using POS.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace POS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TenantAuthorizationFilter()); // Add tenant authorization filter
        }
    }
}
