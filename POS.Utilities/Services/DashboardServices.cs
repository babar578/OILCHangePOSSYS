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

    
        public static DashboardViewModel GetDashboard(DateTime Date)
        {
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

                    string SQL = $"Exec sp_GetDasbroadDaily '{Date}'";
              
                    var Vendor = context.Database.SqlQuery<DashboardViewModel>(SQL).ToList();
                    foreach (var item in Vendor)
                    {
                        returnValue.DiscountCharged = item.DiscountCharged;
                        returnValue.GrossAmount = item.GrossAmount;
                        returnValue.TotalNetAmount = item.TotalNetAmount;
                        returnValue.ReceiptTotalCash = item.ReceiptTotalCash;
                        returnValue.ReceiptTotalCredit = item.ReceiptTotalCredit;
                        returnValue.TOTALBILL = item.TOTALBILL;


                        returnValue.ServiceCharged = item.ServiceCharged;
                        returnValue.DiscountCharged = item.DiscountCharged;
                        returnValue.GstCharged = item.GstCharged;

                    }

                }
            }
            catch (Exception ex)
            {
                var lun = ex.ToString();
            }
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
