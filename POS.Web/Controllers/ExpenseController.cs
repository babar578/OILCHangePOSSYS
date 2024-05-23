using POS.Utilities.Services;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POS.Web.Controllers
{
    public class ExpenseController : Controller
    {
        #region Expense
        public ActionResult Expenses()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Expenses");
        }
        public ActionResult GetAllExpenses()
        {
            var Vendors = ExpenseServices.GetAllExpenses();
            foreach (var item in Vendors)
            {
                int id = Convert.ToInt32(item.CreatedBY);
                var area = UserServices.GetUserById(id);
                item.username = area.UserName;
            }
            return PartialView("_GetAllExpenses", Vendors);
        }
        [HttpGet]
        public ActionResult AddExpense(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            ExpenseViewModel model = new ExpenseViewModel();
            if (id != null)
            {
                model = ExpenseServices.GetExpenseById(id ?? 0);
            }
            return View("_AddExpense", model);
        }
        [HttpPost]
        public JsonResult AddExpense(ExpenseViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            var timeUtc = DateTime.UtcNow;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            try
            {
                bool add;
                if (model.Id == 0)
                {

                    model.IsActive = true;
                    model.CreationDate =today;
                    model.CreatedBY = user.Id;

                    add = ExpenseServices.AddExpense(model);
                }
                else
                {
                    add = ExpenseServices.UpdateExpense(model);
                }

                if (add)
                {
                    message = "Success";
                }
                else
                {
                    message = "Error";
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Json(message);
        }
        [HttpPost]
        public JsonResult DeleteExpense(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ExpenseServices.DeleteExpense(id);
                if (del)
                {
                    message = "Success";
                }
                else
                {
                    message = "Error";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return Json(message);
        }
        #endregion
    }
}