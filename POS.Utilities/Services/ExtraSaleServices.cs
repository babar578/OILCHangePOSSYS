using POS.Database.DatabaseModel;
using POS.Utilities.MultiTenant;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
   public class ExtraSaleServices
    {
        #region Expense Functions

        public static bool AddExpense(ExtraSaleViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    ExtraSale entity = (ExtraSale)model;
                    context.ExtraSales.Add(entity);
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

        public static ExtraSaleViewModel GetExpenseById(int id)
        {
            ExtraSaleViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from ExtraSale where Id={id}";
                    returnValue = context.Database.SqlQuery<ExtraSale>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ExtraSaleViewModel> GetAllExpenses()
        {
            List<ExtraSaleViewModel> returnValue = new List<ExtraSaleViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    //string SQL = $"select *from ExtraSale Where DATEDIFF (DAY , CreationDate , GETDATE()) between 1 and 30 order by CreationDate ASC";
                    string SQL = $"select * from ExtraSale  order by CreationDate ASC";
                    var Clients = context.Database.SqlQuery<ExtraSale>(SQL).ToList();
                    returnValue = Clients.Select(p => (ExtraSaleViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ExtraSaleViewModel> GetExpenses()
        {
            List<ExtraSaleViewModel> returnValue = new List<ExtraSaleViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select Id, Name from ExtraSale";
                    var Users = context.Database.SqlQuery<ExtraSale>(SQL).ToList();
                    returnValue = Users.Select(p => (ExtraSaleViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        //public static bool UpdateExpense(ExpenseViewModel model)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            var find = context.Expenses.Where(p => p.Id == model.Id).SingleOrDefault();
        //            if (find != null)
        //            {
        //                if (!string.IsNullOrWhiteSpace(model.Reason))
        //                    find.Reason = model.Reason;
        //                if (model.CreatedBY > 0)
        //                    find.CreatedBY = model.CreatedBY;
        //                find.IsActive = true;
        //                find.CreationDate = model.CreationDate;
        //                context.SaveChanges();
        //                returnValue = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}

        //public static bool DeleteExpense(int id)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            var del = context.Expenses.Where(p => p.Id == id).SingleOrDefault();
        //            if (del != null)
        //            {
        //                context.Expenses.Remove(del);
        //                context.SaveChanges();
        //                returnValue = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}


        #endregion
    }
}
