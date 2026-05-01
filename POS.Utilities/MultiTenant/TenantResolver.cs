using System;
using System.Configuration;
using System.Data.SqlClient;

namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// Resolves tenant information from ControlDB
    /// </summary>
    public class TenantResolver
    {
        private static readonly string _controlDBConnectionString =
            ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;

        /// <summary>
        /// Resolves tenant by username from ControlDB
        /// </summary>
        public static TenantInfo ResolveTenantByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            try
            {
                using (var connection = new SqlConnection(_controlDBConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT t.TenantId, t.TenantName, t.TenantCode, t.DBServer, 
                               t.DBName, t.DBUser, t.DBPassword, t.IsActive
                        FROM Tenants t
                        INNER JOIN ControlUsers cu ON t.TenantId = cu.TenantId
                        WHERE cu.UserName = @Username AND t.IsActive = 1 AND cu.IsActive = 1";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TenantInfo
                                {
                                    TenantId = reader.GetInt32(0),
                                    TenantName = reader.GetString(1),
                                    TenantCode = reader.GetString(2),
                                    DBServer = reader.GetString(3),
                                    DBName = reader.GetString(4),
                                    DBUser = reader.GetString(5),
                                    DBPassword = reader.GetString(6),
                                    IsActive = reader.GetBoolean(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Error resolving tenant: {ex.Message}");
                throw new Exception("Unable to resolve tenant information", ex);
            }

            return null;
        }

        /// <summary>
        /// Gets tenant by ID from ControlDB
        /// </summary>
        public static TenantInfo GetTenantById(int tenantId)
        {
            try
            {
                using (var connection = new SqlConnection(_controlDBConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT TenantId, TenantName, TenantCode, DBServer, DBName, 
                               DBUser, DBPassword, IsActive
                        FROM Tenants 
                        WHERE TenantId = @TenantId AND IsActive = 1";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TenantId", tenantId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TenantInfo
                                {
                                    TenantId = reader.GetInt32(0),
                                    TenantName = reader.GetString(1),
                                    TenantCode = reader.GetString(2),
                                    DBServer = reader.GetString(3),
                                    DBName = reader.GetString(4),
                                    DBUser = reader.GetString(5),
                                    DBPassword = reader.GetString(6),
                                    IsActive = reader.GetBoolean(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting tenant: {ex.Message}");
                throw;
            }

            return null;
        }
    }
}

