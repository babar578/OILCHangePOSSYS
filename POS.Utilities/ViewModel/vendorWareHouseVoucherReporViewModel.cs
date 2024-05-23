using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class vendorWareHouseVoucherReporViewModel
    {
        public string UserName { get; set; }
        public string VendorName { get; set; }
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string DocNo { get; set; }
        public double GrossAmount { get; set; }
        public double TotalNetAmount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public int DetailId { get; set; }
        public string VoucherLineNo { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Remarks { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalRate { get; set; }
    }
}
