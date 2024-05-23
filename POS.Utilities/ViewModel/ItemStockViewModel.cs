using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ItemStockViewModel
    {


        public Nullable<double> OpeningQuantity { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
        public int ItemId { get; set; }
        public int CategoriesID { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> VendorToWarehouseQuantity { get; set; }
        public Nullable<double> IssueToLocationQuantity { get; set; }
        public Nullable<double> ReturnToWarehouseQuantity { get; set; }
        public Nullable<double> ReturnToVendorQuantity { get; set; }
        public Nullable<double> WastageQuantity { get; set; }
        public Nullable<double> ClosingInventoryQuantity { get; set; }
        public double BalanceQuantity { get; set; }
        public Nullable<double> AverageRate { get; set; }

        //public double TotalRate { get; set; }
        public double Rate { get; set; }
        public double Price { get; set; }
        public Nullable<double> xQuantity { get; set; }
        public Nullable<double> xRemainingQuantity { get; set; }
        public double kitchenblan { get; set; }
        public string LineRemarks { get; set; }
        public Nullable<double> Gst { get; set; }
        public Nullable<double> AdjustmentIN { get; set; }
        public Nullable<double> AdjustmentOUT { get; set; }
    }
}
