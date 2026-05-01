using POS.Utilities.MultiTenant;
using POS.Utilities.ViewModel;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace POS.Utilities.Services
{
    public static class MessagingSettingsService
    {
        #region SMS Settings

        public static SMSSettingsViewModel GetSMSSettings()
        {
            SMSSettingsViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = "SELECT TOP 1 * FROM SMSSettings ORDER BY Id DESC";
                    returnValue = context.Database.SqlQuery<SMSSettingsViewModel>(SQL).FirstOrDefault();
                    
                    // If no record exists, return default disabled settings
                    if (returnValue == null)
                    {
                        returnValue = new SMSSettingsViewModel
                        {
                            Id = 0,
                            IsEnabled = false,
                            ProviderName = "Telenor Corporate SMS",
                            ApiUrl = "https://telenorcsms.com.pk:27677/corporate_sms2/api"
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool SaveSMSSettings(SMSSettingsViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    if (model.Id > 0)
                    {
                        // Update existing
                        string updateSQL = @"
                            UPDATE SMSSettings 
                            SET IsEnabled = @IsEnabled,
                                ProviderName = @ProviderName,
                                ApiUrl = @ApiUrl,
                                ApiKey = @ApiKey,
                                ApiToken = @ApiToken,
                                SenderNumber = @SenderNumber,
                                SenderId = @SenderId,
                                Username = @Username,
                                Password = @Password,
                                Mask = @Mask,
                                AdditionalConfig = @AdditionalConfig,
                                ModifiedDate = GETDATE(),
                                ModifiedBy = @ModifiedBy
                            WHERE Id = @Id";

                        int rowsAffected = context.Database.ExecuteSqlCommand(updateSQL,
                            new SqlParameter("@Id", model.Id),
                            new SqlParameter("@IsEnabled", model.IsEnabled),
                            new SqlParameter("@ProviderName", (object)model.ProviderName ?? DBNull.Value),
                            new SqlParameter("@ApiUrl", (object)model.ApiUrl ?? DBNull.Value),
                            new SqlParameter("@ApiKey", (object)model.ApiKey ?? DBNull.Value),
                            new SqlParameter("@ApiToken", (object)model.ApiToken ?? DBNull.Value),
                            new SqlParameter("@SenderNumber", (object)model.SenderNumber ?? DBNull.Value),
                            new SqlParameter("@SenderId", (object)model.SenderId ?? DBNull.Value),
                            new SqlParameter("@Username", (object)model.Username ?? DBNull.Value),
                            new SqlParameter("@Password", (object)model.Password ?? DBNull.Value),
                            new SqlParameter("@Mask", (object)model.Mask ?? DBNull.Value),
                            new SqlParameter("@AdditionalConfig", (object)model.AdditionalConfig ?? DBNull.Value),
                            new SqlParameter("@ModifiedBy", (object)model.ModifiedBy ?? DBNull.Value)
                        );
                        returnValue = rowsAffected > 0;
                    }
                    else
                    {
                        // Insert new
                        string insertSQL = @"
                            INSERT INTO SMSSettings 
                            (IsEnabled, ProviderName, ApiUrl, ApiKey, ApiToken, SenderNumber, SenderId, Username, Password, Mask, AdditionalConfig, CreatedDate, CreatedBy)
                            VALUES 
                            (@IsEnabled, @ProviderName, @ApiUrl, @ApiKey, @ApiToken, @SenderNumber, @SenderId, @Username, @Password, @Mask, @AdditionalConfig, GETDATE(), @CreatedBy)";

                        int rowsAffected = context.Database.ExecuteSqlCommand(insertSQL,
                            new SqlParameter("@IsEnabled", model.IsEnabled),
                            new SqlParameter("@ProviderName", (object)model.ProviderName ?? DBNull.Value),
                            new SqlParameter("@ApiUrl", (object)model.ApiUrl ?? DBNull.Value),
                            new SqlParameter("@ApiKey", (object)model.ApiKey ?? DBNull.Value),
                            new SqlParameter("@ApiToken", (object)model.ApiToken ?? DBNull.Value),
                            new SqlParameter("@SenderNumber", (object)model.SenderNumber ?? DBNull.Value),
                            new SqlParameter("@SenderId", (object)model.SenderId ?? DBNull.Value),
                            new SqlParameter("@Username", (object)model.Username ?? DBNull.Value),
                            new SqlParameter("@Password", (object)model.Password ?? DBNull.Value),
                            new SqlParameter("@Mask", (object)model.Mask ?? DBNull.Value),
                            new SqlParameter("@AdditionalConfig", (object)model.AdditionalConfig ?? DBNull.Value),
                            new SqlParameter("@CreatedBy", (object)model.CreatedBy ?? DBNull.Value)
                        );
                        returnValue = rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool ToggleSMSEnabled(bool isEnabled)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = "UPDATE SMSSettings SET IsEnabled = @IsEnabled, ModifiedDate = GETDATE() WHERE Id = (SELECT TOP 1 Id FROM SMSSettings ORDER BY Id DESC)";
                    int rowsAffected = context.Database.ExecuteSqlCommand(SQL,
                        new SqlParameter("@IsEnabled", isEnabled)
                    );
                    returnValue = rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion

        #region WhatsApp Settings

        public static WhatsAppSettingsViewModel GetWhatsAppSettings()
        {
            WhatsAppSettingsViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = "SELECT TOP 1 * FROM WhatsAppSettings ORDER BY Id DESC";
                    returnValue = context.Database.SqlQuery<WhatsAppSettingsViewModel>(SQL).FirstOrDefault();
                    
                    // If no record exists, return default disabled settings
                    if (returnValue == null)
                    {
                        returnValue = new WhatsAppSettingsViewModel
                        {
                            Id = 0,
                            IsEnabled = false,
                            ProviderName = "WhatsApp Business API"
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool SaveWhatsAppSettings(WhatsAppSettingsViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    if (model.Id > 0)
                    {
                        // Update existing
                        string updateSQL = @"
                            UPDATE WhatsAppSettings 
                            SET IsEnabled = @IsEnabled,
                                ProviderName = @ProviderName,
                                ApiUrl = @ApiUrl,
                                ApiKey = @ApiKey,
                                ApiToken = @ApiToken,
                                PhoneNumber = @PhoneNumber,
                                SenderNumber = @SenderNumber,
                                InstanceId = @InstanceId,
                                AccessToken = @AccessToken,
                                AdditionalConfig = @AdditionalConfig,
                                ModifiedDate = GETDATE(),
                                ModifiedBy = @ModifiedBy
                            WHERE Id = @Id";

                        int rowsAffected = context.Database.ExecuteSqlCommand(updateSQL,
                            new SqlParameter("@Id", model.Id),
                            new SqlParameter("@IsEnabled", model.IsEnabled),
                            new SqlParameter("@ProviderName", (object)model.ProviderName ?? DBNull.Value),
                            new SqlParameter("@ApiUrl", (object)model.ApiUrl ?? DBNull.Value),
                            new SqlParameter("@ApiKey", (object)model.ApiKey ?? DBNull.Value),
                            new SqlParameter("@ApiToken", (object)model.ApiToken ?? DBNull.Value),
                            new SqlParameter("@PhoneNumber", (object)model.PhoneNumber ?? DBNull.Value),
                            new SqlParameter("@SenderNumber", (object)model.SenderNumber ?? DBNull.Value),
                            new SqlParameter("@InstanceId", (object)model.InstanceId ?? DBNull.Value),
                            new SqlParameter("@AccessToken", (object)model.AccessToken ?? DBNull.Value),
                            new SqlParameter("@AdditionalConfig", (object)model.AdditionalConfig ?? DBNull.Value),
                            new SqlParameter("@ModifiedBy", (object)model.ModifiedBy ?? DBNull.Value)
                        );
                        returnValue = rowsAffected > 0;
                    }
                    else
                    {
                        // Insert new
                        string insertSQL = @"
                            INSERT INTO WhatsAppSettings 
                            (IsEnabled, ProviderName, ApiUrl, ApiKey, ApiToken, PhoneNumber, SenderNumber, InstanceId, AccessToken, AdditionalConfig, CreatedDate, CreatedBy)
                            VALUES 
                            (@IsEnabled, @ProviderName, @ApiUrl, @ApiKey, @ApiToken, @PhoneNumber, @SenderNumber, @InstanceId, @AccessToken, @AdditionalConfig, GETDATE(), @CreatedBy)";

                        int rowsAffected = context.Database.ExecuteSqlCommand(insertSQL,
                            new SqlParameter("@IsEnabled", model.IsEnabled),
                            new SqlParameter("@ProviderName", (object)model.ProviderName ?? DBNull.Value),
                            new SqlParameter("@ApiUrl", (object)model.ApiUrl ?? DBNull.Value),
                            new SqlParameter("@ApiKey", (object)model.ApiKey ?? DBNull.Value),
                            new SqlParameter("@ApiToken", (object)model.ApiToken ?? DBNull.Value),
                            new SqlParameter("@PhoneNumber", (object)model.PhoneNumber ?? DBNull.Value),
                            new SqlParameter("@SenderNumber", (object)model.SenderNumber ?? DBNull.Value),
                            new SqlParameter("@InstanceId", (object)model.InstanceId ?? DBNull.Value),
                            new SqlParameter("@AccessToken", (object)model.AccessToken ?? DBNull.Value),
                            new SqlParameter("@AdditionalConfig", (object)model.AdditionalConfig ?? DBNull.Value),
                            new SqlParameter("@CreatedBy", (object)model.CreatedBy ?? DBNull.Value)
                        );
                        returnValue = rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool ToggleWhatsAppEnabled(bool isEnabled)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = "UPDATE WhatsAppSettings SET IsEnabled = @IsEnabled, ModifiedDate = GETDATE() WHERE Id = (SELECT TOP 1 Id FROM WhatsAppSettings ORDER BY Id DESC)";
                    int rowsAffected = context.Database.ExecuteSqlCommand(SQL,
                        new SqlParameter("@IsEnabled", isEnabled)
                    );
                    returnValue = rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
    }
}





