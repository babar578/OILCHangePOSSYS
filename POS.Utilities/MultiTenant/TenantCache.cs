using System;
using System.Runtime.Caching;

namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// In-memory cache for tenant information to improve performance
    /// </summary>
    public static class TenantCache
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets tenant by ID from cache or database
        /// </summary>
        public static TenantInfo GetTenant(int tenantId)
        {
            string key = $"Tenant_{tenantId}";

            if (_cache.Contains(key))
            {
                return _cache.Get(key) as TenantInfo;
            }

            lock (_lock)
            {
                // Double-check after acquiring lock
                if (_cache.Contains(key))
                {
                    return _cache.Get(key) as TenantInfo;
                }

                // Load from database
                var tenant = TenantResolver.GetTenantById(tenantId);
                if (tenant != null)
                {
                    var policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) // Cache for 1 hour
                    };
                    _cache.Add(key, tenant, policy);
                }

                return tenant;
            }
        }

        /// <summary>
        /// Gets tenant by username from cache or database
        /// </summary>
        public static TenantInfo GetTenantByUsername(string username)
        {
            string key = $"TenantUser_{username}";

            if (_cache.Contains(key))
            {
                return _cache.Get(key) as TenantInfo;
            }

            lock (_lock)
            {
                // Double-check after acquiring lock
                if (_cache.Contains(key))
                {
                    return _cache.Get(key) as TenantInfo;
                }

                // Load from database
                var tenant = TenantResolver.ResolveTenantByUsername(username);
                if (tenant != null)
                {
                    var policy = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) // Cache for 1 hour
                    };
                    _cache.Add(key, tenant, policy);
                    
                    // Also cache by TenantId
                    _cache.Add($"Tenant_{tenant.TenantId}", tenant, policy);
                }

                return tenant;
            }
        }

        /// <summary>
        /// Invalidates a specific tenant from cache
        /// </summary>
        public static void InvalidateTenant(int tenantId)
        {
            string key = $"Tenant_{tenantId}";
            _cache.Remove(key);
            
            // Note: Cannot remove by username without knowing all usernames for this tenant
            // Consider implementing a reverse lookup if needed
        }

        /// <summary>
        /// Clears all tenant entries from cache
        /// </summary>
        public static void Clear()
        {
            foreach (var element in _cache)
            {
                if (element.Key.StartsWith("Tenant_") || element.Key.StartsWith("TenantUser_"))
                {
                    _cache.Remove(element.Key);
                }
            }
        }

        /// <summary>
        /// Gets cache statistics for monitoring
        /// </summary>
        public static CacheStats GetStats()
        {
            int tenantCount = 0;
            int userCount = 0;

            foreach (var element in _cache)
            {
                if (element.Key.StartsWith("Tenant_"))
                    tenantCount++;
                else if (element.Key.StartsWith("TenantUser_"))
                    userCount++;
            }

            return new CacheStats
            {
                TenantCount = tenantCount,
                UserMappingCount = userCount,
                TotalEntries = tenantCount + userCount
            };
        }
    }

    /// <summary>
    /// Cache statistics
    /// </summary>
    public class CacheStats
    {
        public int TenantCount { get; set; }
        public int UserMappingCount { get; set; }
        public int TotalEntries { get; set; }
    }
}

