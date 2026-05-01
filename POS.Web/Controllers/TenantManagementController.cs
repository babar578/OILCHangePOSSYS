using POS.Utilities.MultiTenant;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;

namespace POS.Web.Controllers
{
    /// <summary>
    /// Controller for managing tenants (Admin only)
    /// </summary>
    public class TenantManagementController : Controller
    {
        /// <summary>
        /// Check if current user has admin rights
        /// </summary>
        private bool IsAdmin()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            // TODO: Implement proper admin check based on your UserType or Roles
            // For now, returning true - YOU MUST IMPLEMENT PROPER AUTHORIZATION
            return user != null;
        }

        /// <summary>
        /// View for tenant management
        /// </summary>
        public ActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        /// <summary>
        /// Get all tenants
        /// </summary>
        [HttpGet]
        public JsonResult GetAllTenants()
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Unauthorized" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var tenants = new System.Collections.Generic.List<object>();
                string controlConnString = ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;

                using (var conn = new SqlConnection(controlConnString))
                {
                    conn.Open();
                    string sql = "SELECT TenantId, TenantName, TenantCode, DBServer, DBName, IsActive, CreatedDate FROM Tenants ORDER BY TenantId";
                    var cmd = new SqlCommand(sql, conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tenants.Add(new
                        {
                            TenantId = reader.GetInt32(0),
                            TenantName = reader.GetString(1),
                            TenantCode = reader.GetString(2),
                            DBServer = reader.GetString(3),
                            DBName = reader.GetString(4),
                            IsActive = reader.GetBoolean(5),
                            CreatedDate = reader.GetDateTime(6)
                        });
                    }
                }

                return Json(new { success = true, data = tenants }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Create a new tenant
        /// </summary>
        [HttpPost]
        public JsonResult CreateTenant(string tenantName, string tenantCode, string dbServer, string dbName, string dbUser, string dbPassword)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(tenantName) || string.IsNullOrEmpty(tenantCode) || 
                    string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbName))
                {
                    return Json(new { success = false, message = "All fields are required" });
                }

                // 1. Create new database
                string masterConnString = $"Data Source={dbServer};Initial Catalog=master;User ID={dbUser};Password={dbPassword}";
                using (var conn = new SqlConnection(masterConnString))
                {
                    conn.Open();
                    var cmd = new SqlCommand($"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE [{dbName}]", conn);
                    cmd.ExecuteNonQuery();
                }

                // 2. TODO: Copy schema from template database
                // You can use SQL Server Management Objects (SMO) or execute a schema script
                // For now, this step is manual - you should generate schema script from existing DB

                // 3. Encrypt the password before storing
                string encryptedPassword = TenantSecurityHelper.EncryptPassword(dbPassword);

                // 4. Insert tenant record in ControlDB
                string controlConnString = ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;
                using (var conn = new SqlConnection(controlConnString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO Tenants (TenantName, TenantCode, DBServer, DBName, DBUser, DBPassword, IsActive, CreatedDate)
                                   VALUES (@TenantName, @TenantCode, @DBServer, @DBName, @DBUser, @DBPassword, 1, GETDATE())";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@TenantName", tenantName);
                    cmd.Parameters.AddWithValue("@TenantCode", tenantCode);
                    cmd.Parameters.AddWithValue("@DBServer", dbServer);
                    cmd.Parameters.AddWithValue("@DBName", dbName);
                    cmd.Parameters.AddWithValue("@DBUser", dbUser);
                    cmd.Parameters.AddWithValue("@DBPassword", encryptedPassword);
                    cmd.ExecuteNonQuery();
                }

                // 5. Clear cache to ensure fresh data
                TenantCache.Clear();

                return Json(new { success = true, message = "Tenant created successfully. Please copy the database schema manually." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Update tenant status (activate/deactivate)
        /// </summary>
        [HttpPost]
        public JsonResult UpdateTenantStatus(int tenantId, bool isActive)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                string controlConnString = ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;
                using (var conn = new SqlConnection(controlConnString))
                {
                    conn.Open();
                    string sql = "UPDATE Tenants SET IsActive = @IsActive, ModifiedDate = GETDATE() WHERE TenantId = @TenantId";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    cmd.Parameters.AddWithValue("@TenantId", tenantId);
                    cmd.ExecuteNonQuery();
                }

                // Clear cache for this tenant
                TenantCache.InvalidateTenant(tenantId);

                return Json(new { success = true, message = "Tenant status updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Add user to tenant (map user to ControlUsers)
        /// </summary>
        [HttpPost]
        public JsonResult AddUserToTenant(string userName, int tenantId)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                string controlConnString = ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;
                using (var conn = new SqlConnection(controlConnString))
                {
                    conn.Open();
                    string sql = @"IF NOT EXISTS (SELECT 1 FROM ControlUsers WHERE UserName = @UserName)
                                   INSERT INTO ControlUsers (UserName, TenantId, IsActive) 
                                   VALUES (@UserName, @TenantId, 1)";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@TenantId", tenantId);
                    cmd.ExecuteNonQuery();
                }

                // Clear cache
                TenantCache.Clear();

                return Json(new { success = true, message = "User added to tenant successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Test tenant connection
        /// </summary>
        [HttpPost]
        public JsonResult TestConnection(int tenantId)
        {
            if (!IsAdmin())
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            try
            {
                var tenant = TenantResolver.GetTenantById(tenantId);
                if (tenant == null)
                {
                    return Json(new { success = false, message = "Tenant not found" });
                }

                // Try to connect to tenant database
                using (var conn = new SqlConnection(tenant.GetConnectionString()))
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT 1", conn);
                    cmd.ExecuteScalar();
                }

                return Json(new { success = true, message = "Connection successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Connection failed: " + ex.Message });
            }
        }
    }
}

