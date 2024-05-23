using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class Fn_ReportWareHouseViewModel
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string CategoryNAme { get; set; }
        public Nullable<double> OpeningBalQty { get; set; }
        public Nullable<double> OpeninBalAmount { get; set; }
        public double OpeningBalAvgRate { get; set; }
        public Nullable<double> OpeningQty { get; set; }
        public Nullable<double> OpeningAmount { get; set; }
        public double OpeningAvgRate { get; set; }
        public Nullable<double> PurchaseQty { get; set; }
        public Nullable<double> PurchaseAmount { get; set; }
        public double PurchaseAvgRate { get; set; }
        public Nullable<double> IssuanceQty { get; set; }
        public Nullable<double> IssuanceAmount { get; set; }
        public double IssuanceAvgRate { get; set; }
        public Nullable<double> IssuanceReturnQty { get; set; }
        public Nullable<double> IssuanceReturnAmount { get; set; }
        public double IssuanceReturnAvgRate { get; set; }
        public Nullable<double> PurchaseReturnQty { get; set; }
        public Nullable<double> PurchaseReturnAmount { get; set; }
        public double PurchaseReturnAvgRate { get; set; }
        public Nullable<double> WastageQty { get; set; }
        public Nullable<double> WastageAmount { get; set; }
        public double WastageAvgRate { get; set; }
        public Nullable<double> ClosingQty { get; set; }
        public Nullable<double> ClosingAmount { get; set; }
        public double ClosingAvgRate { get; set; }
        public double AvgRate { get; set; }
        public double Profit { get; set; }
    }
}
