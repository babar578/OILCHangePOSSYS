using POS.Utilities.MultiTenant;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Linq;
using System.Web.UI;

namespace POS.Web.Reports
{
    /// <summary>
    /// Base class for all report pages that ensures tenant context is available
    /// </summary>
    public class ReportBasePage : Page
    {
        public ReportBasePage()
        {
            // Wire up the PreInit event - this is the earliest point where Session is available
            this.PreInit += ReportBasePage_PreInit;
        }

        private void ReportBasePage_PreInit(object sender, EventArgs e)
        {
            // Ensure tenant context is set BEFORE any page processing
            EnsureTenantContext();
        }

        private void EnsureTenantContext()
        {
            try
            {
                // Check if tenant context is already set (by HTTP Module)
                if (TenantContext.HasTenant)
                {
                    System.Diagnostics.Debug.WriteLine("[ReportBasePage] Tenant context already set");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("[ReportBasePage] Tenant context NOT set, attempting to restore from session");

                // Session should be available now
                if (Session == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ReportBasePage] ERROR: Session is NULL!");
                    throw new InvalidOperationException("Session is not available");
                }

                // Check if user is logged in
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ReportBasePage] No user in session, redirecting to login");
                    Response.Redirect("~/Account/Login?reason=no-user", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] User found in session: {user.UserName}");

                // Get tenant ID from session
                var tenantId = Session["TenantId"] as int?;
                
                if (!tenantId.HasValue)
                {
                    System.Diagnostics.Debug.WriteLine("[ReportBasePage] ERROR: No TenantId in session!");
                    System.Diagnostics.Debug.WriteLine("[ReportBasePage] Session keys: " + string.Join(", ", Session.Keys.Cast<string>().ToList()));
                    
                    // Try to re-login user
                    Session.Abandon();
                    Response.Redirect("~/Account/Login?reason=no-tenantid", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] Found TenantId: {tenantId.Value}, loading tenant");

                // Load tenant from cache/database
                var tenant = TenantCache.GetTenant(tenantId.Value);
                
                if (tenant == null || !tenant.IsActive)
                {
                    System.Diagnostics.Debug.WriteLine($"[ReportBasePage] Tenant {tenantId.Value} not found or inactive");
                    Session.Abandon();
                    Response.Redirect("~/Account/Login?reason=invalid-tenant", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                // Set tenant context
                TenantContext.CurrentTenant = tenant;
                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] SUCCESS! Tenant context set for: {tenant.TenantName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] CRITICAL ERROR in EnsureTenantContext:");
                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] Message: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ReportBasePage] Stack trace: {ex.StackTrace}");
                
                // Show error page or redirect
                try
                {
                    Response.Write($"<h3 style='color:red'>Error Setting Tenant Context</h3>");
                    Response.Write($"<p>{ex.Message}</p>");
                    Response.Write($"<p><a href='/Account/Login'>Return to Login</a></p>");
                    Response.End();
                }
                catch
                {
                    // If response writing fails, try redirect
                    Response.Redirect("~/Account/Login?reason=error", false);
                }
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            // Don't clear tenant context here - other parts of the pipeline might need it
            base.OnUnload(e);
        }
    }
}

