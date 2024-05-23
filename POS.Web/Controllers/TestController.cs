using PagedList;
using POS.Utilities.ReportsModel;
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
    public class TestController : Controller
    {
        // GET: Test
        #region VendorPayment

        [HttpGet]
        public ActionResult FilterVendorPayment()
        {
            var model = new FilterVendorPayment();
            return PartialView("_FilterVendorPayment", model);
        }
        public ActionResult VendorPayment(FilterVendorPayment model)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (model.FromDate != DateTime.MinValue && model.ToDate != DateTime.MinValue)
            {
                Session["_VendorPayment"] = model;
            }
            return View();
        }
        public ActionResult GetVendorPayment()
        {
            return PartialView("_GetVendorPayment");
        }
        #endregion
    }
}