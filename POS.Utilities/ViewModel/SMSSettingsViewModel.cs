using System;

namespace POS.Utilities.ViewModel
{
    public class SMSSettingsViewModel
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public string ProviderName { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiToken { get; set; }
        public string SenderNumber { get; set; }
        public string SenderId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mask { get; set; }
        public string AdditionalConfig { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}





