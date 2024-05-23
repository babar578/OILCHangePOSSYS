
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VenderPaymentLedgerSummaryViewModel
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public double TotalBuying { get; set; }
        public double TotalPayment { get; set; }
        public double RemainingBalance { get; set; }
    }
}
