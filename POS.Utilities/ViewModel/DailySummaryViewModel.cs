using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class DailySummaryViewModel
    {
        public double CashSales { get; set; }
        public double CreditSales { get; set; }
        public double TotalSales { get; set; }
        public double SalesReturn { get; set; }
        public double NetSales { get; set; }
        public double TotalExpenses { get; set; }
        public double TotalPurchases { get; set; }
        public double NetCashInHand { get; set; } // CashSales - Expenses (simplified)
    }
}
