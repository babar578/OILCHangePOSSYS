using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ReportsModel
{
    public class InventoryBalanceViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string UnitName { get; set; }
        public Nullable<double> AverageRate { get; set; }
        public Nullable<double> OpeningQuantity { get; set; }
        public Nullable<double> OpeningBalanceQuantity { get; set; }
        public Nullable<double> VendorToWarehouseQuantity { get; set; }
        public Nullable<double> IssueToLocationQuantity { get; set; }
        public Nullable<double> ReturnToWarehouseQuantity { get; set; }
        public Nullable<double> ReturnToVendorQuantity { get; set; }
        public Nullable<double> WastageQuantity { get; set; }
        public Nullable<double> ClosingInventoryQuantity { get; set; }
        public Nullable<double> ClosingBalanceQuantity { get; set; }
        public Nullable<double> InventoryValue { get; set; }

    }
}
