using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public   class Sp_StockInCashReport
    {
        public int Id { get; set; }
        public Nullable<double> AlignmentAmount { get; set; }
        public Nullable<double> wheelBalanceAmount { get; set; }
        public Nullable<double> TPMS { get; set; }
        public Nullable<double> DiscountPercentage { get; set; }
        public Nullable<double> NitrogenGas { get; set; }
        public Nullable<double> withoutserviceChager { get; set; }
        public Nullable<double> WithServiceCarger { get; set; }
        public Nullable<double> ReceiptTotalCash { get; set; }
        public Nullable<double> ReceiptTotalCredit { get; set; }
        public Nullable<double> TotalNetAmount { get; set; }
        public Nullable<int> ExpanceAmount { get; set; }
        public Nullable<int> ExtraSaleAmount { get; set; }
        public Nullable<double> VendorPaymentsAmount { get; set; }
    }
}
