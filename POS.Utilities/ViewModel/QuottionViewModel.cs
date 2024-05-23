using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public  class QuottionViewModel
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string DocNo { get; set; }
        public Nullable<double> GrossAmount { get; set; }
        public Nullable<double> TotalNetAmount { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public int DetailId { get; set; }
        public string VoucherLineNo { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalRate { get; set; }
    }
}
