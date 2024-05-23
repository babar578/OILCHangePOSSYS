using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VendorWhereHouseViewModel
    {



        public string VendorName { get; set; }
        public double Rate { get; set; }
        public double PRate { get; set; }
        public double TotalRate { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Unit { get; set; }
        public string DocNo { get; set; }
        public double Quantity { get; set; }
        public double GrossAmount { get; set; }
        public double GstCharges { get; set; }
        public double TotalNetAmount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public double Discount { get; set; }
        public string Remarks1 { get; set; }
        public double SurCharge { get; set; }
        public string ItemName { get; set; }
    }
}
