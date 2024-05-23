using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class POSOderSaleReportViewModel

    {

        public string InvoiceNumber { get; set; }
        public double TotalNetAmount { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
    }
}
