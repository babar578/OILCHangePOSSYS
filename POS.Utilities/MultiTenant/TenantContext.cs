using System.Web;

namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// Thread-safe storage for current tenant context using HttpContext.Items
    /// </summary>
    public static class TenantContext
    {
        private const string TenantInfoKey = "CurrentTenantInfo";

        /// <summary>
        /// Gets or sets the current tenant for the request
        /// </summary>
        public static TenantInfo CurrentTenant
        {
            get
            {
                if (HttpContext.Current?.Items[TenantInfoKey] != null)
                {
                    return HttpContext.Current.Items[TenantInfoKey] as TenantInfo;
                }
                return null;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[TenantInfoKey] = value;
                }
            }
        }

        /// <summary>
        /// Clears the tenant context
        /// </summary>
        public static void Clear()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(TenantInfoKey);
            }
        }

        /// <summary>
        /// Checks if a tenant context is currently set
        /// </summary>
        public static bool HasTenant => CurrentTenant != null;
    }
}

