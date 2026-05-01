using POS.Database.DatabaseModel;
using System;

namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// Factory for creating DbContext instances with tenant-specific connection strings
    /// </summary>
    public static class MultiTenantDbContextFactory
    {
        /// <summary>
        /// Creates a DbContext for the current tenant
        /// </summary>
        public static POSEntities CreateDbContext()
        {
            var tenant = TenantContext.CurrentTenant;

            if (tenant == null)
            {
                throw new InvalidOperationException("No tenant context available. Ensure tenant is resolved before accessing database.");
            }

            return CreateDbContext(tenant);
        }

        /// <summary>
        /// Creates a DbContext for a specific tenant
        /// </summary>
        public static POSEntities CreateDbContext(TenantInfo tenant)
        {
            if (tenant == null)
                throw new ArgumentNullException(nameof(tenant));

            var connectionString = tenant.GetEntityConnectionString();
            return new POSEntities(connectionString);
        }
    }
}

