using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
   public class VenderPaymentLedgeReportView
    {


        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Narration { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public double TotalNetBalance { get; set; }
        public double TotalPayment { get; set; }
        public double RemainingBalance { get; set; }








    }
}
