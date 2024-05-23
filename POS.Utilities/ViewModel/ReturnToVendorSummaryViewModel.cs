using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
 public   class ReturnToVendorSummaryViewModel
    {

        public Double TotalQunity { get; set; }
        public string ItemName { get; set; }

        public string VendorName { get; set; }
        public DateTime TransactionDate { get; set; }


    }
}
