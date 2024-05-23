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
    public class VendorController : Controller
    {
        #region Vendor
        public ActionResult Vendors()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Vendors");
        }

        public ActionResult GetAllVendors()
        {
            var Vendors = VendorServices.GetAllVendors();
            return PartialView("_GetAllVendors", Vendors);
        }
        [HttpGet]
        public ActionResult AddVendor(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            VendorViewModel model = new VendorViewModel();
            if (id != null)
            {
                model = VendorServices.GetVendorById(id ?? 0);
            }
            return View("_AddVendor", model);
        }
        [HttpPost]
        public JsonResult AddVendor(VendorViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.BusinessStartDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.IsActive = true;
                    add = VendorServices.AddVendor(model);
                }
                else
                {
                    add = VendorServices.UpdateVendor(model);
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
        public JsonResult DeleteVendor(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteVendor(id);
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
        #region VendorPayment
        public ActionResult VendorPayments()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("VendorPayments");
        }
        public ActionResult GetAllVendorPayments()
        {
            var VendorPayments = VendorServices.GetAllVendorPayments();
            return PartialView("_GetAllVendorPayments", VendorPayments);
        }
        public ActionResult VendorPayment()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PartialVendorPayment(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            VendorPaymentViewModel model = new VendorPaymentViewModel();

            if (id != null)
            {
                model = VendorServices.GetVendorPaymentById(id ?? 0);
            }

            return View("_PartialVendorPayment", model);
        }


        [HttpGet]
        public ActionResult PartialVendorPaymentLedger(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            DateTime fromDate = new DateTime(1900, 1, 1);
            DateTime toDate = DateTime.Today;
            List<VenderPaymentLedgerViewModel> model = VendorServices.GetVenderPaymentLedger(fromDate, toDate, id.Value);
            return View("_PartialVendorPaymentLedger", model);
        }


        [HttpPost]
        public JsonResult AddVendorPayment(VendorPaymentViewModel model)
        {
            string message = string.Empty;

            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;

            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;

                    add = VendorServices.AddVendorPayment(model);
                }
                else
                {
                    add = VendorServices.UpdateVendorPayment(model);
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
        public JsonResult DeleteVendorPayment(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteVendorPayment(id);
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

         #region City

        public ActionResult Cities()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            return View("Cities");
        }

        public ActionResult GetAllCities()
        {

            var Cities = VendorServices.GetAllCities();
            return View("_GetAllCities", Cities);
        }

        [HttpGet]
        public ActionResult City(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            CityViewModel model = new CityViewModel();

            if (id != null)
            {
                model = VendorServices.GetCityById(id ?? 0);
            }

            return View("_City", model);
        }

        [HttpPost]
        public JsonResult AddCity(CityViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.IsActive = true;
                    add = VendorServices.AddCity(model);
                }
                else
                {
                    add = VendorServices.UpdateCity(model);
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
        public JsonResult DeleteCity(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteCity(id);
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

        #region Location

        public ActionResult Locations()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Locations");
        }

        public ActionResult GetAllLocations()
        {
            var Locations = VendorServices.GetAllLocations();
            return PartialView("_GetAllLocations", Locations);
        }


        [HttpGet]
        public ActionResult Location(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            LocationViewModel model = new LocationViewModel();

            if (id != null)
            {
                model = VendorServices.GetLocationById(id ?? 0);
            }

            return View("_Location", model);
        }

        [HttpPost]
        public JsonResult AddLocation(LocationViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.IsActive = true;
                    add = VendorServices.AddLocation(model);
                }
                else
                {
                    add = VendorServices.UpdateLocation(model);
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
        public JsonResult DeleteLocation(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteLocation(id);
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

        #region Raw Items

        public ActionResult RawItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }

        public ActionResult GetAllRawItems()
        {
            var items = ItemServices.GetAllItems(true);

            return PartialView("_GetAllRawItems", items);
        }

        [HttpGet]
        public ActionResult RawItem(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ItemViewModel model = new ItemViewModel();
            if (id != null)
            {
                model = ItemServices.GetItemById(id ?? 0);
            }

            return View("_RawItem", model);
        }

        #endregion

        #region Raw Category


        public ActionResult RawCategories()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }

        public ActionResult GetAllRawCategories()
        {
            var categories = ItemServices.GetAllCategories(true);
            return PartialView("_GetAllRawCategories", categories);
        }

        [HttpGet]
        public ActionResult RawCategory(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            CategoryViewModel model = new CategoryViewModel();

            if (id != null)
            {
                model = ItemServices.GetCategoryById(id ?? 0);
            }

            return View("_RawCategory", model);
        }
        #endregion
        #region repot
        public ActionResult VendorsPayments()
        {
            return View();
        }
        public ActionResult VendorsPaymentsSummary()
        {
            return View();
        }
        #endregion
        #region Customer
        public ActionResult Customers()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("Customers");
        }

        public ActionResult GetAllCustomers()
        {
            var Vendors = VendorServices.GetAllCustomers();
            return PartialView("_GetAllCustomers", Vendors);
        }
        [HttpGet]
        public ActionResult AddCustomer(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            CustomerViewModel model = new CustomerViewModel();
            if (id != null)
            {
                model = VendorServices.GetCustomerById(id ?? 0);
            }
            return View("_Customers", model);
        }
        [HttpPost]
        public JsonResult AddCustomer(CustomerViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                  //  model.BusinessStartDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.IsActive = true;
                    add = VendorServices.AddCustomer(model);
                }
                else
                {
                    add = VendorServices.UpdateCustomer(model);
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
        public JsonResult DeleteCustomer(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteCustomer(id);
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