using POS.Utilities.ViewModel;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace POS.Utilities.Services
{
    /// <summary>
    /// Service for managing website leads in ControlDB
    /// </summary>
    public static class LeadService
    {
        private static readonly string _controlDBConnectionString =
            ConfigurationManager.ConnectionStrings["ControlDB"].ConnectionString;

        /// <summary>
        /// Creates a new website lead
        /// </summary>
        public static bool CreateLead(WebsiteLeadViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var connection = new SqlConnection(_controlDBConnectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO WebsiteLeads 
                        (Id, FullName, Company, Email, Phone, Message, InterestedPlan, Source, Status, Country, Language, CreatedAt, IsActive)
                        VALUES 
                        (@Id, @FullName, @Company, @Email, @Phone, @Message, @InterestedPlan, @Source, @Status, @Country, @Language, @CreatedAt, @IsActive)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@FullName", model.FullName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Company", (object)model.Company ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", model.Email ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Phone", (object)model.Phone ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Message", model.Message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@InterestedPlan", (object)model.InterestedPlan ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Source", (object)model.Source ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Status", model.Status ?? "New");
                        command.Parameters.AddWithValue("@Country", (object)model.Country ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Language", model.Language ?? "en");
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@IsActive", true);

                        int rowsAffected = command.ExecuteNonQuery();
                        returnValue = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating lead: {ex.Message}");
                throw;
            }
            return returnValue;
        }

        /// <summary>
        /// Gets all leads (for admin portal)
        /// </summary>
        public static System.Collections.Generic.List<WebsiteLeadViewModel> GetAllLeads()
        {
            var leads = new System.Collections.Generic.List<WebsiteLeadViewModel>();
            try
            {
                using (var connection = new SqlConnection(_controlDBConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT Id, FullName, Company, Email, Phone, Message, InterestedPlan, Source, 
                               Status, AssignedTo, Notes, FollowUpDate, Country, Language, 
                               CreatedAt, LastUpdated, IsActive
                        FROM WebsiteLeads
                        WHERE IsActive = 1
                        ORDER BY CreatedAt DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                leads.Add(new WebsiteLeadViewModel
                                {
                                    Id = reader.GetGuid(0),
                                    FullName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    Company = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Message = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    InterestedPlan = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Source = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Status = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    AssignedTo = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9),
                                    Notes = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    FollowUpDate = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                                    Country = reader.IsDBNull(12) ? null : reader.GetString(12),
                                    Language = reader.IsDBNull(13) ? null : reader.GetString(13),
                                    CreatedAt = reader.GetDateTime(14),
                                    LastUpdated = reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                                    IsActive = reader.GetBoolean(16)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting leads: {ex.Message}");
                throw;
            }
            return leads;
        }
    }
}

