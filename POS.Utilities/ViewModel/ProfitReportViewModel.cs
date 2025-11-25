using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ProfitReportViewModel
    {
        public double TotalRevenue { get; set; }
        public double TotalCostOfGoods { get; set; } // Purchases in this context
        public double TotalExpenses { get; set; }
        public double GrossProfit { get; set; } // Revenue - COGS
        public double NetProfit { get; set; } // GrossProfit - Expenses
    }
}
