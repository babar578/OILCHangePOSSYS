using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalCloseOrders { get; set; }
        public int TotalOpenOrders { get; set; }
        public int TotalDraftOrders { get; set; }
        public int TotalUsers { get; set; }
        public int TotalTables { get; set; }
        public int TotalItems { get; set; }
        public int TotalMenuCategories { get; set; }

        public Nullable< int> TOTALBILL { get; set; }
        public Nullable<double> GstCharged { get; set; }
        public Nullable<double > DiscountCharged { get; set; }
        public Nullable<double> ServiceCharged { get; set; }
        public Nullable<double > GrossAmount { get; set; }
        public Nullable<double > TotalNetAmount { get; set; }
        public Nullable<double > ReceiptTotalCash { get; set; }
        public Nullable<double > ReceiptTotalCredit { get; set; }

    }
}
