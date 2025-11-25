using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class DashboardDataViewModel
    {
        public string Title { get; set; }
        public double Val { get; set; }
    }

    public class Dashboard01ViewModel
    {
        public Dashboard01ViewModel()
        {
            Top10ItemsSoldByQty = new List<DashboardItemViewModel>();
            Top10ItemsSoldByAmount = new List<DashboardItemViewModel>();
            HourlyInvoiceAmountPattern = new List<DashboardHourlyDataViewModel>();
            HourlyInvoiceCountPattern = new List<DashboardHourlyDataViewModel>();
        }

        public double AmountReceived { get; set; }
        public double TotalNetSales { get; set; }
        public double TotalExpenses { get; set; }
        public int NoOfInvoices { get; set; }
        public int NoOfSalesReturns { get; set; }
        public double TotalPurchases { get; set; }
        public int NoOfUnpaidInvoices { get; set; }
        public double TotalSalesReturn { get; set; }
        public double HourlyInvoiceAmount { get; set; }
        public int HourlyInvoiceCount { get; set; }
        public List<DashboardItemViewModel> Top10ItemsSoldByQty { get; set; }
        public List<DashboardItemViewModel> Top10ItemsSoldByAmount { get; set; }
        public List<DashboardHourlyDataViewModel> HourlyInvoiceAmountPattern { get; set; }
        public List<DashboardHourlyDataViewModel> HourlyInvoiceCountPattern { get; set; }
    }

    public class Dashboard02ViewModel
    {
        public int NoOfInvoices { get; set; }
        public double AvgInvoicesPerDay { get; set; }
        public double TotalInvoicesVal { get; set; }
        public double AvgInvoiceValePerDay { get; set; }
        public int NoOfPurchase { get; set; }
        public double AvgPurchasePerDay { get; set; }
        public double TotalPurchaseVal { get; set; }
        public double AvgPurchaseValePerDay { get; set; }
        public int NoOfExpenses { get; set; }
        public double AvgExpensePerDay { get; set; }
        public double TotalExpenseVal { get; set; }
        public double AvgExpensValePerDay { get; set; }
    }

    public class DashboardItemViewModel
    {
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
    }

    public class DashboardHourlyDataViewModel
    {
        public int Hour { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
    }
}

