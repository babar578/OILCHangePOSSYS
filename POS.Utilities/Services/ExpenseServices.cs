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
   public class ExpenseServices
    {
        #region Expense Functions

        public static bool AddExpense(ExpenseViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    Expense entity = (Expense)model;
                    context.Expenses.Add(entity);
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

        public static ExpenseViewModel GetExpenseById(int id)
        {
            ExpenseViewModel returnValue = null;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"select * from Expense where Id={id}";
                    returnValue = context.Database.SqlQuery<Expense>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ExpenseViewModel> GetAllExpenses()
        {
            List<ExpenseViewModel> returnValue = new List<ExpenseViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    //string SQL = $"select *from Expense Where DATEDIFF (DAY , CreationDate , GETDATE()) between 1 and 30 order by CreationDate ASC";
                    // string SQL = $"select * from Expense Where Month(CreationDate) = {DateTime.Now.Month} order by CreationDate ASC";
                    string SQL = $"select * from Expense  order by CreationDate ASC";
                    var Clients = context.Database.SqlQuery<Expense>(SQL).ToList();
                    returnValue = Clients.Select(p => (ExpenseViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<ExpenseViewModel> GetExpenses()
        {
            List<ExpenseViewModel> returnValue = new List<ExpenseViewModel>();
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string SQL = $"Select Id, Name from Expense";
                    var Users = context.Database.SqlQuery<Expense>(SQL).ToList();
                    returnValue = Users.Select(p => (ExpenseViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateExpense(ExpenseViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var find = context.Expenses.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Reason))
                            find.Reason = model.Reason;
                        if (model.CreatedBY > 0)
                            find.CreatedBY = model.CreatedBY;
                        find.IsActive = true;
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

        public static bool DeleteExpense(int id)
        {
            bool returnValue = false;
            try
            {
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    var del = context.Expenses.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Expenses.Remove(del);
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
