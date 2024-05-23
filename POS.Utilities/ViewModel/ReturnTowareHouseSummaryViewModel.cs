using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public  class ReturnTowareHouseSummaryViewModel
    {

        public int DepartmentId { get; set; }

        public DateTime TransactionDate { get; set; }
        public string Name { get; set; }

        public double TotalQunity { get; set; }
        public string ItemName { get; set; }

    }
}
