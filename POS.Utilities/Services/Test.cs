using POS.Database.DatabaseModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public static class Test
    {

        #region VendorPayment Functions

        public static bool AddVendorPayment(VendorPaymentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    VendorPayment entity = (VendorPayment)model;
                    context.VendorPayments.Add(entity);
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

        public static VendorPaymentViewModel GetVendorPaymentById(int id)
        {
            VendorPaymentViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorPayments where Id={id}";
                    returnValue = context.Database.SqlQuery<VendorPayment>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetVendorPaymentNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from VendorPayments where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorPaymentViewModel> GetAllVendorPayments(string search = null)
        {
            List<VendorPaymentViewModel> returnValue = new List<VendorPaymentViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VendorPayments";
                    var VendorPayment = context.Database.SqlQuery<VendorPayment>(SQL).ToList();
                    returnValue = VendorPayment.Select(p => (VendorPaymentViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<VendorPaymentViewModel> GetVendorPaymentsByName()
        {
            List<VendorPaymentViewModel> returnValue = new List<VendorPaymentViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from VendorPayments";
                    var VendorPayment = context.Database.SqlQuery<VendorPayment>(SQL).ToList();
                    returnValue = VendorPayment.Select(p => (VendorPaymentViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateVendorPayment(VendorPaymentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.VendorPayments.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (model.VendorId > 0)
                            find.VendorId = model.VendorId;
                        if (!string.IsNullOrEmpty(model.Remarks))
                            find.Remarks = model.Remarks;
                        if (!string.IsNullOrEmpty(model.DocNo))
                            find.DocNo = model.DocNo;
                        if (model.Payment > 0)
                            find.Payment = model.Payment;
                        if (model.RemainingBalance > 0)
                            find.RemainingBalance = model.RemainingBalance;
                        if (model.TransactionDate != DateTime.MinValue)
                            find.TransactionDate = model.TransactionDate;
                        if (model.ModifyDate != DateTime.MinValue)
                            find.ModifyDate = model.ModifyDate;

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

        public static bool DeleteVendorPayment(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.VendorPayments.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.VendorPayments.Remove(del);
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
