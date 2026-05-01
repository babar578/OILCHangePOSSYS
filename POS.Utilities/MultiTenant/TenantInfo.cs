namespace POS.Utilities.MultiTenant
{
    /// <summary>
    /// Represents tenant information including database connection details
    /// </summary>
    public class TenantInfo
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantCode { get; set; }
        public string DBServer { get; set; }
        public string DBName { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Builds a SQL Server connection string for this tenant
        /// Automatically decrypts the password if it's encrypted
        /// </summary>
        public string GetConnectionString()
        {
            // Decrypt password if it appears to be encrypted
            var password = TenantSecurityHelper.IsEncrypted(DBPassword) 
                ? TenantSecurityHelper.DecryptPassword(DBPassword) 
                : DBPassword;
                
            return $"Data Source={DBServer};Initial Catalog={DBName};User ID={DBUser};Password={password};MultipleActiveResultSets=True";
        }

        /// <summary>
        /// Builds an Entity Framework connection string for this tenant
        /// </summary>
        public string GetEntityConnectionString()
        {
            var sqlConnection = GetConnectionString();
            return $"metadata=res://*/DatabaseModel.PosModel.csdl|res://*/DatabaseModel.PosModel.ssdl|res://*/DatabaseModel.PosModel.msl;provider=System.Data.SqlClient;provider connection string=\"{sqlConnection};App=EntityFramework\"";
        }
    }
}

