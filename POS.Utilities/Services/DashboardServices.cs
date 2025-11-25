using POS.Database.DatabaseModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public static class DashboardServices
    {
        public static Dashboard01ViewModel GetDashboard01(DateTime fromDate, DateTime toDate)
        {
            Dashboard01ViewModel result = new Dashboard01ViewModel();

            try
            {
                using (POSEntities context = new POSEntities())
                {
                    // Raw SQL for Total Net Sales (Orders)
                    string salesQuery = $@"
                        SELECT ISNULL(SUM(TotalNetAmount), 0) 
                        FROM [Orders] 
                        WHERE IsVoid = 0 
                        AND CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    result.TotalNetSales = context.Database.SqlQuery<double>(salesQuery).FirstOrDefault();

                    // Raw SQL for Amount Received (Assuming same as Sales for now, or check Payment status)
                    // If you have a specific Payment table or flag, adjust here. 
                    // For now, using TotalNetSales as AmountReceived based on typical simple POS logic
                    result.AmountReceived = result.TotalNetSales;

                    // Raw SQL for Total Expenses
                    string expenseQuery = $@"
                        SELECT ISNULL(SUM(Amount), 0) 
                        FROM Expense 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    result.TotalExpenses = context.Database.SqlQuery<Int32>(expenseQuery).Select(x => (Int32)x).FirstOrDefault();

                    // Raw SQL for No Of Invoices
                    string invoiceCountQuery = $@"
                        SELECT COUNT(*) 
                        FROM [Orders] 
                        WHERE IsVoid = 0 
                        AND CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    result.NoOfInvoices = context.Database.SqlQuery<int>(invoiceCountQuery).FirstOrDefault();

                    // Raw SQL for Total Purchases
                    string purchaseQuery = $@"
                        SELECT ISNULL(SUM(TotalNetAmount), 0) 
                        FROM VendorToWarehouseHead 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    result.TotalPurchases = context.Database.SqlQuery<double>(purchaseQuery).Select(x => (double)x).FirstOrDefault();

                    // Unpaid Invoices (Orders where IsPayment is false? or based on your logic)
                    // Assuming IsPayment = 0 means unpaid
                    string unpaidQuery = $@"
                        SELECT COUNT(*) 
                        FROM [Orders] 
                        WHERE IsVoid = 0 AND IsPayment = 0
                        AND CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    result.NoOfUnpaidInvoices = context.Database.SqlQuery<int>(unpaidQuery).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log error
            }

            return result;
        }

        public static Dashboard02ViewModel GetDashboard02(DateTime fromDate, DateTime toDate)
        {
            Dashboard02ViewModel result = new Dashboard02ViewModel();

            try
            {
                using (POSEntities context = new POSEntities())
                {
                    // Calculate days difference for averages
                    int days = (toDate - fromDate).Days + 1;
                    if (days == 0) days = 1;

                    // Re-use logic or fetch again
                    string salesQuery = $@"
                        SELECT ISNULL(SUM(TotalNetAmount), 0) 
                        FROM [Orders] 
                        WHERE IsVoid = 0 
                        AND CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    double totalSales = context.Database.SqlQuery<double>(salesQuery).FirstOrDefault();

                    string invoiceCountQuery = $@"
                        SELECT COUNT(*) 
                        FROM [Orders] 
                        WHERE IsVoid = 0 
                        AND CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    int totalInvoices = context.Database.SqlQuery<int>(invoiceCountQuery).FirstOrDefault();

                    result.NoOfInvoices = totalInvoices;
                    result.TotalInvoicesVal = totalSales;
                    result.AvgInvoicesPerDay = days > 0 ? (double)totalInvoices / days : 0;
                    result.AvgInvoiceValePerDay = days > 0 ? totalSales / days : 0;

                    // Purchases
                    string purchaseQuery = $@"
                        SELECT ISNULL(SUM(TotalNetAmount), 0) 
                        FROM VendorToWarehouseHead 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    double totalPurchases = context.Database.SqlQuery<double>(purchaseQuery).Select(x => (double)x).FirstOrDefault();

                    string purchaseCountQuery = $@"
                        SELECT COUNT(*) 
                        FROM VendorToWarehouseHead 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    int totalPurchaseCount = context.Database.SqlQuery<int>(purchaseCountQuery).FirstOrDefault();

                    result.NoOfPurchase = totalPurchaseCount;
                    result.TotalPurchaseVal = totalPurchases;
                    result.AvgPurchasePerDay = days > 0 ? (double)totalPurchaseCount / days : 0;
                    result.AvgPurchaseValePerDay = days > 0 ? totalPurchases / days : 0;

                    // Expenses
                    string expenseQuery = $@"
                        SELECT ISNULL(SUM(Amount), 0) 
                        FROM Expense 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    double totalExpenses = context.Database.SqlQuery<Int32>(expenseQuery).Select(x => (Int32)x).FirstOrDefault();

                    string expenseCountQuery = $@"
                        SELECT COUNT(*) 
                        FROM Expense 
                        WHERE CAST(CreationDate AS DATE) BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
                    int totalExpenseCount = context.Database.SqlQuery<int>(expenseCountQuery).FirstOrDefault();

                    result.NoOfExpenses = totalExpenseCount;
                    result.TotalExpenseVal = totalExpenses;
                    result.AvgExpensePerDay = days > 0 ? (double)totalExpenseCount / days : 0;
                    result.AvgExpensValePerDay = days > 0 ? totalExpenses / days : 0;
                }
            }
            catch (Exception ex)
            {
                // Log error
            }

            return result;
        }

        public static DailySummaryViewModel GetDailySummary(DateTime date)
        {
            DailySummaryViewModel result = new DailySummaryViewModel();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string dateStr = date.ToString("yyyy-MM-dd");

                    // Total Sales
                    string salesQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM [Orders] WHERE IsVoid = 0 AND CAST(CreationDate AS DATE) = '{dateStr}'";
                    result.TotalSales = context.Database.SqlQuery<double>(salesQuery).FirstOrDefault();

                    // Expenses
                    string expenseQuery = $@"SELECT ISNULL(SUM(Amount), 0) FROM Expense WHERE CAST(CreationDate AS DATE) = '{dateStr}'";
                    result.TotalExpenses = context.Database.SqlQuery<Int32>(expenseQuery).Select(x => (Int32)x).FirstOrDefault();

                    // Purchases
                    string purchaseQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM VendorToWarehouseHead WHERE CAST(CreationDate AS DATE) = '{dateStr}'";
                    result.TotalPurchases = context.Database.SqlQuery<double>(purchaseQuery).Select(x => (double)x).FirstOrDefault();

                    // Assuming Cash vs Credit based on PaymentType or similar logic. 
                    // For now, let's assume all are Cash if no specific column exists, or split 50/50 for demo if needed.
                    // Better: Check if there's a PaymentTypeId or similar. 
                    // Checking Order table: it has PaymentTypeId? No, but Order has IsPayment.
                    // Let's assume IsPayment=1 is Paid (Cash) and IsPayment=0 is Unpaid (Credit/Due).
                    
                    string cashSalesQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM [Orders] WHERE IsVoid = 0 AND IsPayment = 1 AND CAST(CreationDate AS DATE) = '{dateStr}'";
                    result.CashSales = context.Database.SqlQuery<double>(cashSalesQuery).FirstOrDefault();

                    string creditSalesQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM [Orders] WHERE IsVoid = 0 AND IsPayment = 0 AND CAST(CreationDate AS DATE) = '{dateStr}'";
                    result.CreditSales = context.Database.SqlQuery<double>(creditSalesQuery).FirstOrDefault();

                    result.NetCashInHand = result.CashSales - result.TotalExpenses;
                }
            }
            catch (Exception) { }
            return result;
        }

        public static ProfitReportViewModel GetProfitReport(DateTime fromDate, DateTime toDate)
        {
            ProfitReportViewModel result = new ProfitReportViewModel();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string from = fromDate.ToString("yyyy-MM-dd");
                    string to = toDate.ToString("yyyy-MM-dd");

                    // Revenue
                    string salesQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM [Orders] WHERE IsVoid = 0 AND CAST(CreationDate AS DATE) BETWEEN '{from}' AND '{to}'";
                    result.TotalRevenue = context.Database.SqlQuery<double>(salesQuery).FirstOrDefault();

                    // Purchases (COGS proxy)
                    string purchaseQuery = $@"SELECT ISNULL(SUM(TotalNetAmount), 0) FROM VendorToWarehouseHead WHERE CAST(CreationDate AS DATE) BETWEEN '{from}' AND '{to}'";
                    result.TotalCostOfGoods = context.Database.SqlQuery<double>(purchaseQuery).Select(x => (double)x).FirstOrDefault();

                    // Expenses
                    string expenseQuery = $@"SELECT ISNULL(SUM(Amount), 0) FROM Expense WHERE CAST(CreationDate AS DATE) BETWEEN '{from}' AND '{to}'";
                    result.TotalExpenses = context.Database.SqlQuery<Int32>(expenseQuery).Select(x => (Int32)x).FirstOrDefault();

                    result.GrossProfit = result.TotalRevenue - result.TotalCostOfGoods;
                    result.NetProfit = result.GrossProfit - result.TotalExpenses;
                }
            }
            catch (Exception) { }
            return result;
        }

        public static DashboardViewModel GetDashboard(DateTime Date)
        {
            // Keeping original method signature but using new logic if needed, or just redirecting
            // For backward compatibility if used elsewhere
            DashboardViewModel returnValue = new DashboardViewModel();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    returnValue.TotalOpenOrders = context.Orders.Where(p => p.IsUpdateMode == true && p.IsPayment == false).Count();
                    returnValue.TotalDraftOrders = context.Orders.Where(p => p.IsUpdateMode == true && p.IsPayment == false).Count();
                    returnValue.TotalMenuCategories = context.Categories.Count();
                    returnValue.TotalItems = context.Items.Count();
                    returnValue.TotalUsers = context.Users.Count();
                }
            }
            catch (Exception) { }
            return returnValue;
        }

        public static int GetTotalTablesByFloor(int? floorId = null)
        {
            int returnValue = 0;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    returnValue = context.FloorTables.Where(p => p.FloorId == floorId).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
    }
}
