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
 
    public class QuotationsController : Controller
    {
        // GET: Quotations
        #region Quotations
        public ActionResult Quotations()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult QuotationHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            QuotationheadViewModel model = new QuotationheadViewModel();

            if (id != null)
            {
                model = VendorServices.GetQuotationHeadById(id ?? 0);
            }

            return PartialView("_QuotationHead", model);
        }
        public ActionResult QuotationDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            QuotationDetailViewModel model = new QuotationDetailViewModel();
            return PartialView("_QuotationDetail", model);
        }
        public void SearchItem(int? ItemId)
        {
            var item = ItemServices.GetItemcurrentById(ItemId ?? 0);

            List<ItemViewModel> items = new List<ItemViewModel>();
            List<ItemViewModel> sessionItems = Session[WebUtil.SearchItems] as List<ItemViewModel>;
            if (sessionItems?.Count > 0)
            {
                sessionItems.Add(item);
            }
            else
            {
                items.Add(item);
                Session[WebUtil.SearchItems] = items;
            }
        }
        public void UpdateQuotation(int ItemId, float Price, float Quantity)
        {
            List<ItemViewModel> sessionItems = Session[WebUtil.SearchItems] as List<ItemViewModel>;
            ItemViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.Id == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    if (Price > 0 && Quantity > 0)
                    {
                        searchItem.Price = Math.Round(Price, 2);
                        searchItem.Quantity = Math.Round(Quantity, 2);

                    }

                    Session[WebUtil.SearchItems] = sessionItems;
                }
            }
        }
        public void DeleteQuotation(int ItemId)
        {
            List<ItemViewModel> sessionItems = Session[WebUtil.SearchItems] as List<ItemViewModel>;
            ItemViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.Id == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    sessionItems.Remove(searchItem);
                    Session[WebUtil.SearchItems] = sessionItems;
                }
            }
        }
        [HttpPost]
        public JsonResult AddQuotation(QuotationheadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            var re = "oder punched by:" + user.UserName;
            ResponseDto response = new ResponseDto();
            var timeUtc = DateTime.UtcNow;
            model.TransactionDate = DateTime.Today;
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            try
            {
                // bool add = false;
                if (model.Id == 0)
                {
                    model.TransactionDate = today.Date;
                    model.CreationDate = today;
                    model.ModifyDate = today;
                    model.CreatedBy = user.Id;
                    model.Remarks = re;

                    model.DocNo = WebUtil.GetInvoiceNumber();
                    response = VendorServices.AddQuoations(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (response.Status == true)
                {
                    message = "Success";

                    Session[WebUtil.SearchItems] = null;
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
            return Json(response);
            //   return Json(response);
        }
        public ActionResult QuotationItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("QuotationItems");
        }
        public ActionResult GetAllQuotationItems()
        {
            List<QuotationheadViewModel> model = VendorServices.GetQuotationDetailHeads();
            return PartialView("_GetAllQuotationItems", model);
        }
        public ActionResult QuotationVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["PurchaseVoucherID"] = Id;
            }

            return View("QuotationVoucherSummary");
        }
        public ActionResult GetQuotationVoucherSummary()
        {
            QuotationheadViewModel model = new QuotationheadViewModel();

            if (TempData["PurchaseVoucherID"] != null)
            {
                int Id = (int)TempData["PurchaseVoucherID"];
                model = VendorServices.GetQuotationHeadById(Id);
                if (model != null)
                {
                    model.QuotationDetails = VendorServices.GetQuotationHeadDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetQuotationVoucherSummary", model);
        }
     
      
        #endregion

        


    }




}