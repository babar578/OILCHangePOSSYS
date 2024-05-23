using POS.Database.DatabaseModel;
using POS.Utilities.ReportsModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public class FmoServices
    {
        #region Purchase
        public static bool AddPurchase(PurchaseViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Purchase entity = (Purchase)model;
                    context.Purchases.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static PurchaseViewModel GetPurchaseById(int id)
        {
            PurchaseViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {

                    string SQL = $"select * from Purchases where Id={id}";
                    returnValue = context.Database.SqlQuery<Purchase>(SQL).SingleOrDefault();
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
                using (POSEntities context = new POSEntities())
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

        public static string GetPurchasePaymentById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Payment from Purchases where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<PurchaseViewModel> GetAllPurchase(string search = null)
        {
            List<PurchaseViewModel> returnValue = new List<PurchaseViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Purchases";
                    var Vendor = context.Database.SqlQuery<Purchase>(SQL).ToList();
                    returnValue = Vendor.Select(p => (PurchaseViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdatePurchase(PurchaseViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Purchases.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Payment))
                            find.Payment = model.Payment;
                        find.TransationDate = model.TransationDate;
                        find.ModificationDate = model.ModificationDate;
                        find.CreationDate = model.CreationDate;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool DeletePurchase(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Purchases.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Purchases.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion

        #region process
        public static bool Addprocess(ProcessViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Process entity = (Process)model;
                    context.Processes.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static ProcessViewModel GetprocessById(int id)
        {
            ProcessViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {

                    string SQL = $"select * from Process where Id={id}";
                    returnValue = context.Database.SqlQuery<Process>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetProcessPaymentById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Payment from Process where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ProcessViewModel> GetAllprocess(string search = null)
        {
            List<ProcessViewModel> returnValue = new List<ProcessViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Process";
                    var Vendor = context.Database.SqlQuery<Process>(SQL).ToList();
                    returnValue = Vendor.Select(p => (ProcessViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool Updateprocess(ProcessViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Processes.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Payment))
                            find.Payment = model.Payment;
                        find.TransationDate = model.TransationDate;
                        find.ModificationDate = model.ModificationDate;
                        find.CreationDate = model.CreationDate;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool Deleteprocess(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Processes.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Processes.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion

        #region Sale
        public static bool AddSale(SaleViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Sale entity = (Sale)model;
                    context.Sales.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static SaleViewModel GetSaleById(int id)
        {
            SaleViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {

                    string SQL = $"select * from Sale where Id={id}";
                    returnValue = context.Database.SqlQuery<Sale>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetSalePaymentById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Payment from Sale where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<SaleViewModel> GetAllSale(string search = null)
        {
            List<SaleViewModel> returnValue = new List<SaleViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Sale";
                    var Vendor = context.Database.SqlQuery<Sale>(SQL).ToList();
                    returnValue = Vendor.Select(p => (SaleViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateSale(SaleViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Sales.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Payment))
                        find.Payment = model.Payment;
                        find.TransationDate = model.TransationDate;
                        find.ModificationDate = model.ModificationDate;
                        find.CreationDate = model.CreationDate;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool DeleteSale(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Sales.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Sales.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
    }
}
