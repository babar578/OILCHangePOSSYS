
using CrystalDecisions.CrystalReports.Engine;
using POS.Database.DatabaseModel;
using POS.Utilities.MultiTenant;
using POS.Utilities.ReportsModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public static class ReportServices
    {


        public static List<Sp_StockInCashReport> GetSockinHandCashReport(DateTime fromDate, DateTime toDate)
        {
            List<Sp_StockInCashReport> returnValue = new List<Sp_StockInCashReport>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<Sp_StockInCashReport>("Exec sp_DailyCashReport  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<POSOderSaleReportViewModel> GetPOSSaleReport(DateTime fromDate, DateTime toDate)
        {
            List<POSOderSaleReportViewModel> returnValue = new List<POSOderSaleReportViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<POSOderSaleReportViewModel>("Exec POSOderSaleReport  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #region Inventory

        public static List<ReturnTowareHouseSummaryViewModel> GetReturnTowareHouseSummaryReport(DateTime fromDate, DateTime toDate)
        {
            List<ReturnTowareHouseSummaryViewModel> returnValue = new List<ReturnTowareHouseSummaryViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<ReturnTowareHouseSummaryViewModel>("execute  ReturnVenderTowareHouseSummary  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }



        //public static List<GetHistoryCarVoucherViewModel> GetAllOrders(int CarID)
        //{
        //    List<GetHistoryCarVoucherViewModel> returnValue = new List<GetHistoryCarVoucherViewModel>();
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            string SQL = $"Exec GetHistoryCarVoucher where Id={CarID}";
        //            var orders = context.Database.SqlQuery<GetHistoryCarVoucher_Result>(SQL).ToList();
        //            returnValue = orders.Select(p => (GetHistoryCarVoucherViewModel) p).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}


        public static List<GetHistoryCarVoucherViewModel> GetCarHistoryreportReport(int ID)
        {
            List<GetHistoryCarVoucherViewModel> returnValue = new List<GetHistoryCarVoucherViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (ID > 0)
                    {
                        returnValue = context.Database.SqlQuery<GetHistoryCarVoucherViewModel>("execute  GetHistoryCarVoucher  @ID",
                   
                        new SqlParameter("@ID", SqlDbType.Int) { Value = ID }).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ReturnToWareHouseViewModel> GetReturnTowareHouseReport(DateTime fromDate, DateTime toDate, int? itemId = null, int? categoryId = null)
        {
            List<ReturnToWareHouseViewModel> returnValue = new List<ReturnToWareHouseViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        // Stored procedure expects: @Todate, @fromdate, @itemID, @CategoryID
                        returnValue = context.Database.SqlQuery<ReturnToWareHouseViewModel>("execute ReturnToWareHouse @Todate, @fromdate, @itemID, @CategoryID",
                        new SqlParameter("@Todate", SqlDbType.DateTime) { Value = toDate },
                        new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@itemID", SqlDbType.Int) { Value = (object)itemId ?? DBNull.Value },
                        new SqlParameter("@CategoryID", SqlDbType.Int) { Value = (object)categoryId ?? DBNull.Value }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        public static List<ReturnToVendorSummaryViewModel> GetReturnToVendorSummaryReport(DateTime fromDate, DateTime toDate)
        {
            List<ReturnToVendorSummaryViewModel> returnValue = new List<ReturnToVendorSummaryViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<ReturnToVendorSummaryViewModel>("execute  ReturnVenderSummary  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }




        public static List<GetHistoryCarVoucherViewModel> GetCustomerHistoreyById(int id)
        {
            List<GetHistoryCarVoucherViewModel> returnValue = new List<GetHistoryCarVoucherViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {

                    string SQL = $"Exec GetHistoryCarVoucher Id={id}";
                    var Vendor = context.Database.SqlQuery<GetHistoryCarVoucher_Result>(SQL).ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ReturnToVendorViewModel> GetReturnToVendorReport(DateTime fromDate, DateTime toDate)
        {
            List<ReturnToVendorViewModel> returnValue = new List<ReturnToVendorViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<ReturnToVendorViewModel>("execute  ReturnToVendor  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ComsumptionReportViewModel> GetComsumptionReport(DateTime fromDate, DateTime toDate)
        {
            List<ComsumptionReportViewModel> returnValue = new List<ComsumptionReportViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<ComsumptionReportViewModel>("execute  Comsumption  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();
                        foreach (var item in returnValue)
                        {
                            item.Rate = Math.Round(item.Rate, 2);

                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }



        public static List<VenderPaymentLedgerSummaryViewModel> GetVendorPaymentSummaryReport(DateTime fromDate, DateTime toDate, int? venderId = null)
        {
            List<VenderPaymentLedgerSummaryViewModel> returnValue = new List<VenderPaymentLedgerSummaryViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        // Stored procedure expects: @Todate, @fromdate, @VenderId
                        returnValue = context.Database.SqlQuery<VenderPaymentLedgerSummaryViewModel>("execute VenderPaymentLedgerSummary @Todate, @fromdate, @VenderId",
                        new SqlParameter("@Todate", SqlDbType.DateTime) { Value = toDate },
                        new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@VenderId", SqlDbType.Int) { Value = (object)venderId ?? DBNull.Value }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<WastageDetailViewModel> GetWastageReport(DateTime fromDate, DateTime toDate)
        {
            List<WastageDetailViewModel> returnValue = new List<WastageDetailViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<WastageDetailViewModel>("execute  WastageRepot  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<IssueToLocationDetailViewModel> GetIssueToDeptmentReport(DateTime fromDate, DateTime toDate)
        {
            List<IssueToLocationDetailViewModel> returnValue = new List<IssueToLocationDetailViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<IssueToLocationDetailViewModel>("execute  IssueToDeptment  @fromDate , @toDate",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate }).ToList();
                    }
                    foreach (var item in returnValue)
                    {
                        item.Rate = Math.Round(item.Rate, 2);
                        item.TotalRate = Math.Round(item.TotalRate, 2);
                        item.Quantity = Math.Round(item.Quantity, 2);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorToWarehouseHeadViewModel> GetVenderToWarHouseReport(DateTime fromDate, DateTime toDate ,int ItemId)
        {
            List<VendorToWarehouseHeadViewModel> returnValue = new List<VendorToWarehouseHeadViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<VendorToWarehouseHeadViewModel>("execute  VenderToWarhouse  @fromDate , @toDate ,@ItemId",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                         new SqlParameter("@ItemId", SqlDbType.Int) { Value = ItemId },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate}).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        //public static List<InventoryBalanceViewModel> GetInventoryBalanceReport(DateTime fromDate, DateTime toDate, int? ItemId, int? UnitId)
        //{
        //    List<InventoryBalanceViewModel> returnValue = new List<InventoryBalanceViewModel>();
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            StringBuilder SQL = new StringBuilder();
        //            if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
        //            {
        //                SQL.AppendLine("SELECT * FROM ");
        //                SQL.AppendLine($"fn_InventoryBal('{fromDate}','{toDate}') ");
        //                SQL.AppendLine("WHERE 1=1 ");
        //            }
        //            if (ItemId.HasValue)
        //            {
        //                SQL.Append($"AND ItemId = {ItemId}");
        //            }
        //            if (UnitId.HasValue)
        //            {
        //                SQL.Append($"AND UnitId = {UnitId}");
        //            }

        //            returnValue = context.Database.SqlQuery<InventoryBalanceViewModel>(SQL.ToString()).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }s
        //    return returnValue;
        //}
      public static List<VenderPaymentLedgeReportView> GetVenderPaymentLedgeReport(DateTime fromDate, DateTime toDate, int? ItemId, int? UnitId)
        {
            List<VenderPaymentLedgeReportView> returnValue = new List<VenderPaymentLedgeReportView>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        SQL.AppendLine("SELECT * FROM ");
                        SQL.AppendLine($"fn_VenderPaymentLedger('{fromDate}','{toDate}') ");

                        SQL.AppendLine("WHERE 1=1 ");
                    }
                    if (ItemId.HasValue)
                    {
                        SQL.Append($"AND ItemId = {ItemId}");
                    }
                    if (UnitId.HasValue)
                    {
                        SQL.Append($"AND UnitId = {UnitId}");
                    }

                    returnValue = context.Database.SqlQuery<VenderPaymentLedgeReportView>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VendorPaymentViewModel> GetVendorPaymentReport(DateTime fromDate, DateTime toDate, int? VendorId)
        {
            List<VendorPaymentViewModel> returnValue = new List<VendorPaymentViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        SQL.AppendLine("SELECT * FROM ");
                        SQL.AppendLine($" VendorPayments WHERE TransactionDate Between '{fromDate}'  AND '{toDate}' ");
                    }
                    if (VendorId.HasValue)
                    {
                        SQL.Append($"AND VendorId = {VendorId}");
                    }
                    returnValue = context.Database.SqlQuery<VendorPaymentViewModel>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        public static List<Fn_ReportWareHouseViewModel> GetInventoryBalanceReport(DateTime fromDate, DateTime toDate, int? ItemId, int? UnitId)
        {
            List<Fn_ReportWareHouseViewModel> returnValue = new List<Fn_ReportWareHouseViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        SQL.AppendLine("SELECT * ");
                        SQL.AppendLine($"FROM fn_ReportViewStockinhand_WareHouse('{fromDate:MM/dd/yyyy}','{toDate:MM/dd/yyyy}') ");
                        SQL.AppendLine("WHERE 1=1 ");
                    }
                    if (ItemId.HasValue)
                    {
                        SQL.Append($"AND ItemId = {ItemId}");
                    }
                    if (UnitId.HasValue)
                    {
                        SQL.Append($"AND UnitId = {UnitId}");
                    }

                    returnValue = context.Database.SqlQuery<Fn_ReportWareHouseViewModel>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<WastageDetailViewModel> GetWastageReport(DateTime fromDate, DateTime toDate, int ItemId)
        {
            List<WastageDetailViewModel> returnValue = new List<WastageDetailViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<WastageDetailViewModel>("execute  WastageRepot  @fromDate , @toDate ,@ItemId",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate },
                            new SqlParameter("@ItemId", SqlDbType.Int) { Value = ItemId }
                        ).ToList();

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<IssueToLocationDetailViewModel> GetIssueToDeptmentReport(DateTime fromDate, DateTime toDate, int itemid, int CategoriesID)
        {


            List<IssueToLocationDetailViewModel> returnValue = new List<IssueToLocationDetailViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<IssueToLocationDetailViewModel>("execute  IssueToDeptment  @fromDate , @toDate, @itemid,@CategoriesID ",
                        new SqlParameter("@fromDate", SqlDbType.DateTime) { Value = fromDate },
                              new SqlParameter("@toDate", SqlDbType.DateTime) { Value = toDate },
                       new SqlParameter("@itemid", SqlDbType.Int) { Value = itemid },
                            new SqlParameter("@CategoriesID", SqlDbType.Int) { Value = CategoriesID }).ToList();
                    }
                    foreach (var item in returnValue)
                    {
                        item.Rate = Math.Round(item.Rate, 2);
                        item.TotalRate = Math.Round(item.TotalRate, 2);
                        item.Quantity = Math.Round(item.Quantity, 2);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorWhereHouseViewModel> GetVenderToWarHouseReport(DateTime fromDate, DateTime toDate, int ItemId, int VenderId)
        {
            List<VendorWhereHouseViewModel> returnValue = new List<VendorWhereHouseViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        returnValue = context.Database.SqlQuery<VendorWhereHouseViewModel>("execute VenderToWarhouse @Todate, @fromdate, @itemid, @VenderID",
                        new SqlParameter("@Todate", SqlDbType.DateTime) { Value = toDate },
                        new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = fromDate },
                        new SqlParameter("@itemid", SqlDbType.Int) { Value = ItemId },
                        new SqlParameter("@VenderID", SqlDbType.Int) { Value = VenderId }).ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VenderPaymentLedgeReportView> GetVenderPaymentLedgeReport(DateTime fromDate, DateTime toDate, int? ItemId, int? UnitId, int VenderId)
        {
            List<VenderPaymentLedgeReportView> returnValue = new List<VenderPaymentLedgeReportView>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    StringBuilder SQL = new StringBuilder();
                    if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                    {
                        SQL.AppendLine("SELECT * FROM ");
                        SQL.AppendLine($"fn_VenderPaymentLedger('{fromDate:MM/dd/yyyy}','{toDate:MM/dd/yyyy}') ");




                    }
                    if (VenderId > 0)
                    {
                        SQL.AppendLine($"WHERE fn_VenderPaymentLedger.VendorId= {VenderId} ");
                    }


                    if (ItemId.HasValue)
                    {
                        SQL.Append($"AND ItemId = {ItemId}");
                    }
                    if (UnitId.HasValue)
                    {
                        SQL.Append($"AND UnitId = {UnitId}");
                    }

                    returnValue = context.Database.SqlQuery<VenderPaymentLedgeReportView>(SQL.ToString()).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        #region Reporting
        public static DataSet GetInvoiceReport(int id)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString;
                SqlConnection con = new SqlConnection(conStr);
                string SQL = $"Select * from Orders h INNER JOIN OrderItems d ON(h.Id = d.OrderId) where h.Id = {id}";
                SqlCommand cmd = new SqlCommand(SQL, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable PrintBill(string InvoiceNo)
        {
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["Entities"].ConnectionString;
                SqlConnection con = new SqlConnection(conStr);

                string SQL = $"select h.*, d.OrderId, d.ItemId, d.ItemName,	d.Quantity, d.Price As ItemPrice,	" +
                    $"d.TotalPrice As ItemTotalPrice, d.Remarks As ItemRemarks,d.IsVoid As ItemIsVoid , d.IsComplimentary As ItemIsComplimentary," +
                    $"	d.Reason As ItemReasonfrom from Orders h INNER JOIN OrderItems d ON(h.Id = d.OrderId) where h.InvoiceNumber = '{InvoiceNo}'";

                SqlCommand cmd = new SqlCommand(SQL, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                //da.Fill(ds);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion
    }
}
