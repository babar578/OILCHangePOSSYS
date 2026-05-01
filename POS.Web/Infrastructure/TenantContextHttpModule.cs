using POS.Utilities.MultiTenant;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Linq;
using System.Web;

namespace POS.Web.Infrastructure
{
    /// <summary>
    /// HTTP Module that ensures tenant context is set for ALL requests (MVC and WebForms)
    /// This runs before any page/controller processing
    /// </summary>
    public class TenantContextHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            // Hook into the request processing pipeline
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            // Skip for static files
            if (IsStaticFile(context.Request.Path))
                return;

            // Skip for login and logout actions (both GET and POST)
            string path = context.Request.Path.ToLower();
            if (path.Contains("/account/login") || path.Contains("/account/logout"))
                return;

            // Skip if session is not available yet
            if (context.Session == null)
                return;

            try
            {
                // Check if user is authenticated
                var user = context.Session?[WebUtil.CURRENT_USER] as UserViewModel;

                if (user == null)
                {
                    // User not logged in - redirect to login for protected pages only
                    // Don't redirect for Account controller actions or Home/Index (landing page after login)
                    if (!path.Contains("/account/") && !path.Contains("/home/index"))
                    {
                        context.Response.Redirect("~/Account/Login", false);
                        context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    // For /home/index or /account/*, let MVC filter handle authentication
                    return;
                }

                // User is logged in - ALWAYS ensure tenant context is set
                // This is critical for reports and WebForms pages
                if (!TenantContext.HasTenant)
                {
                    // Try to get tenant from session
                    var tenantId = context.Session?["TenantId"] as int?;

                    if (tenantId.HasValue)
                    {
                        try
                        {
                            // Load tenant from cache/database
                            var tenant = TenantCache.GetTenant(tenantId.Value);
                            
                            if (tenant != null && tenant.IsActive)
                            {
                                TenantContext.CurrentTenant = tenant;
                                System.Diagnostics.Debug.WriteLine($"[TenantModule] Tenant context set for {tenant.TenantName} on path: {path}");
                            }
                            else
                            {
                                // Tenant not found or inactive
                                System.Diagnostics.Debug.WriteLine($"[TenantModule] Tenant {tenantId} not found or inactive");
                                
                                // Only redirect if not on account controller or landing page
                                if (!path.Contains("/account/") && !path.Contains("/home/index"))
                                {
                                    context.Session?.Abandon();
                                    context.Response.Redirect("~/Account/Login", false);
                                    context.ApplicationInstance.CompleteRequest();
                                    return;
                                }
                            }
                        }
                        catch (Exception tenantEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[TenantModule] Error loading tenant {tenantId}: {tenantEx.Message}");
                            
                            // Try to resolve tenant directly if cache fails
                            try
                            {
                                var tenant = TenantResolver.GetTenantById(tenantId.Value);
                                if (tenant != null && tenant.IsActive)
                                {
                                    TenantContext.CurrentTenant = tenant;
                                    System.Diagnostics.Debug.WriteLine($"[TenantModule] Tenant context set via resolver for {tenant.TenantName}");
                                }
                            }
                            catch (Exception resolverEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"[TenantModule] Resolver also failed: {resolverEx.Message}");
                            }
                        }
                    }
                    else
                    {
                        // CRITICAL: No tenant in session - this should not happen for authenticated users
                        System.Diagnostics.Debug.WriteLine($"[TenantModule] WARNING: User authenticated but NO TenantId in session for path: {path}");
                        System.Diagnostics.Debug.WriteLine($"[TenantModule] Session Keys: {string.Join(", ", context.Session.Keys.Cast<string>())}");
                        
                        // For reports and other pages, this is a problem - redirect to login
                        if (!path.Contains("/account/") && !path.Contains("/home/index"))
                        {
                            System.Diagnostics.Debug.WriteLine($"[TenantModule] Redirecting to login - no tenant in session");
                            context.Session?.Abandon();
                            context.Response.Redirect("~/Account/Login?reason=no-tenant", false);
                            context.ApplicationInstance.CompleteRequest();
                            return;
                        }
                    }
                }
                else
                {
                    // Tenant context already set (probably from a previous request in the pipeline)
                    System.Diagnostics.Debug.WriteLine($"[TenantModule] Tenant context already set for path: {path}");
                }
            }
            catch (Exception ex)
            {
                // Log error with full details
                System.Diagnostics.Debug.WriteLine($"[TenantModule] CRITICAL ERROR in TenantContextHttpModule:");
                System.Diagnostics.Debug.WriteLine($"[TenantModule] Path: {context.Request.Path}");
                System.Diagnostics.Debug.WriteLine($"[TenantModule] Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[TenantModule] Stack trace: {ex.StackTrace}");
                
                // Try to get session info for debugging
                try
                {
                    if (context.Session != null)
                    {
                        var user = context.Session[WebUtil.CURRENT_USER];
                        var tenantId = context.Session["TenantId"];
                        System.Diagnostics.Debug.WriteLine($"[TenantModule] Session - User: {user != null}, TenantId: {tenantId}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[TenantModule] Session is NULL");
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine($"[TenantModule] Cannot read session");
                }
                
                // For critical pages like reports, we cannot proceed without tenant context
                // Redirect to login if we're on a protected page
                string requestPath = context.Request.Path.ToLower();
                if (requestPath.Contains("/reports/") || requestPath.Contains(".aspx"))
                {
                    System.Diagnostics.Debug.WriteLine($"[TenantModule] Critical error on report/aspx page - redirecting to login");
                    context.Response.Redirect("~/Account/Login?reason=error", false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            // Clean up tenant context at end of request
            // Note: Don't clear here as it might be needed for response processing
        }

        private bool IsStaticFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            string extension = System.IO.Path.GetExtension(path).ToLower();
            
            return extension == ".css" || extension == ".js" || extension == ".jpg" || 
                   extension == ".jpeg" || extension == ".png" || extension == ".gif" || 
                   extension == ".ico" || extension == ".woff" || extension == ".woff2" || 
                   extension == ".ttf" || extension == ".svg" || extension == ".map";
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}

