using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public  class ReturnToWareHouseViewModel
    {




        public int HeadID { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Quantity { get; set; }
        public string ItemName { get; set; }

    }
}
