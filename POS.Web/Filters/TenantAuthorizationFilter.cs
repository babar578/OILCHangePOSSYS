using POS.Utilities.MultiTenant;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System.Web;
using System.Web.Mvc;

namespace POS.Web.Filters
{
    /// <summary>
    /// Global action filter that ensures tenant context is available for MVC actions
    /// Note: TenantContextHttpModule handles tenant setup for all requests (including WebForms)
    /// This filter provides additional validation for MVC controllers
    /// </summary>
    public class TenantAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Skip for login action
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            if (controller == "Account" && (action == "Login"))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Check if user is logged in
            var user = HttpContext.Current.Session[WebUtil.CURRENT_USER] as UserViewModel;

            if (user == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            // Verify tenant context is set (should be set by HTTP module)
            if (!TenantContext.HasTenant)
            {
                // Try to restore tenant from session as fallback
                var tenantId = HttpContext.Current.Session["TenantId"] as int?;

                if (tenantId.HasValue)
                {
                    var tenant = TenantCache.GetTenant(tenantId.Value);
                    if (tenant != null)
                    {
                        TenantContext.CurrentTenant = tenant;
                    }
                    else
                    {
                        // Tenant no longer exists or is inactive
                        HttpContext.Current.Session.Abandon();
                        filterContext.Result = new RedirectResult("~/Account/Login");
                        return;
                    }
                }
                else
                {
                    // No tenant information available
                    HttpContext.Current.Session.Abandon();
                    filterContext.Result = new RedirectResult("~/Account/Login");
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}

