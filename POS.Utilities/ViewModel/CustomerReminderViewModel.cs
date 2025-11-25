using System;

namespace POS.Utilities.ViewModel
{
    public class CustomerReminderViewModel
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CarNumber { get; set; }
        public string Mobile { get; set; }
        public int NoOfDays { get; set; }
        public DateTime ActionDate { get; set; }
    }
}

