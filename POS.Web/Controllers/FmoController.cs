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
    public class FmoController : Controller
    {
        #region Purchase
        public ActionResult Purchases()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Purchases");
        }

        public ActionResult GetAllPurchase()
        {
            var Vendors = FmoServices.GetAllPurchase();
            return PartialView("_GetAllPurchase", Vendors);
        }
        [HttpGet]
        public ActionResult AddPurchase(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            PurchaseViewModel model = new PurchaseViewModel();
            if (id != null)
            {
                model = FmoServices.GetPurchaseById(id ?? 0);
            }
            return View("_AddPurchase", model);
        }
        [HttpPost]
        public JsonResult AddPurchase(PurchaseViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModificationDate = DateTime.Now;
                    model.IsActive = true;
                    add = FmoServices.AddPurchase(model);
                }
                else
                {
                    add = FmoServices.UpdatePurchase(model);
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
        public JsonResult DeletePurchase(int id)
        {
            string message = string.Empty;
            try
            {
                var del = FmoServices.DeletePurchase(id);
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

        #region Process
        public ActionResult Process()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Process");
        }

        public ActionResult GetAllprocess()
        {
            var Vendors = FmoServices.GetAllprocess();
            return PartialView("_GetAllprocess", Vendors);
        }
        [HttpGet]
        public ActionResult Addprocess(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            ProcessViewModel model = new ProcessViewModel();
            if (id != null)
            {
                model = FmoServices.GetprocessById(id ?? 0);
            }
            return View("_AddPurchase", model);
        }
        [HttpPost]
        public JsonResult Addprocess(ProcessViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModificationDate = DateTime.Now;
                    model.IsActive = true;
                    add = FmoServices.Addprocess(model);
                }
                else
                {
                    add = FmoServices.Updateprocess(model);
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
        public JsonResult Deleteprocess(int id)
        {
            string message = string.Empty;
            try
            {
                var del = FmoServices.Deleteprocess(id);
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

        #region Process
        public ActionResult Sale()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Sale");
        }

        public ActionResult GetAllSale()
        {
            var Vendors = FmoServices.GetAllSale();
            return PartialView("_GetAllSale", Vendors);
        }
        [HttpGet]
        public ActionResult AddSale(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            SaleViewModel model = new SaleViewModel();
            if (id != null)
            {
                model = FmoServices.GetSaleById(id ?? 0);
            }
            return View("_AddPurchase", model);
        }
        [HttpPost]
        public JsonResult AddSale (SaleViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModificationDate = DateTime.Now;
                    model.IsActive = true;
                    add = FmoServices.AddSale(model);
                }
                else
                {
                    add = FmoServices.UpdateSale(model);
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
        public JsonResult DeleteSale(int id)
        {
            string message = string.Empty;
            try
            {
                var del = FmoServices.DeleteSale(id);
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