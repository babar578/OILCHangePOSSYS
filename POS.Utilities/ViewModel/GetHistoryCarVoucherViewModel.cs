using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public partial class GetHistoryCarVoucherViewModel
    {
        public string CustomerName { get; set; }
        public string CarNumber { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string Reading { get; set; }
        public string Mobile { get; set; }
        public double TotalPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public double GstCharged { get; set; }
        public double DiscountCharged { get; set; }
        public double ServiceCharged { get; set; }
        public double GrossAmount { get; set; }
        public double TotalNetAmount { get; set; }
        public double ReceiptTotalCash { get; set; }
        public double ReceiptTotalCredit { get; set; }
        public System.DateTime CreationDate { get; set; }

        public string DateDate { get; set; }
    }
}
