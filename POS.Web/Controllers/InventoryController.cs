using POS.Utilities.Services;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using VMS.Utilities.ViewModel;

namespace POS.Web.Controllers
{
    public class InventoryController : Controller
    {
        #region VendorToWarehouse
        public ActionResult VendorToWarehouse()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult VendorToWarehouseHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            VendorToWarehouseHeadViewModel model = new VendorToWarehouseHeadViewModel();

            if (id != null)
            {
                model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }

            return PartialView("_VendorToWarehouseHead", model);
        }
        public ActionResult VendorToWarehouseDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            VendorToWarehouseDetailViewModel model = new VendorToWarehouseDetailViewModel();
            if (refId != null)
            {

            }

            return PartialView("_VendorToWarehouseDetail" , model);
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
        public void UpdateVendorToWarehouse(int ItemId, float PPrice, float Price, float Quantity)
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
                        searchItem.PPrice = Math.Round(PPrice, 2);
                        searchItem.Quantity = Math.Round(Quantity, 2);
                        
                    }

                    Session[WebUtil.SearchItems] = sessionItems;
                }
            }
        }
        public void DeleteVendorToWarehouse(int ItemId)
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
        public JsonResult AddVendorToWarehouse(VendorToWarehouseHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            ResponseDto response = new ResponseDto();
            try
            {
               // bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    response = VendorServices.AddVendorToWarehouse(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (response.Status==true)
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
        public ActionResult WarehouseItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("WarehouseItems");
        }
        public ActionResult GetAllWarehouseItems()
        {
            List<VendorToWarehouseHeadViewModel> model = VendorServices.GetVendorToWarehouseHeads();
            return PartialView("_GetAllWarehouseItems", model);
        }
        public ActionResult WarehouseVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["PurchaseVoucherID"] = Id;
            }

            return View("WarehouseVoucherSummary");
        }
        public ActionResult GetWarehouseVoucherSummary()
        {
            VendorToWarehouseHeadViewModel model = new VendorToWarehouseHeadViewModel();

            if (TempData["PurchaseVoucherID"] != null)
            {
                int Id = (int)TempData["PurchaseVoucherID"];
                model = VendorServices.GetVendorToWarehouseHeadById(Id);
                if (model != null)
                {
                    model.VendorToWarehouseDetails = VendorServices.GetVendorToWarehouseDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetWarehouseVoucherSummary", model);
        }
        [HttpPost]
        public JsonResult DeleteWarehouseVoucherRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteVendorToWarehouse(id);
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

        #region IssueItemsToLocation
        public ActionResult IssueItemsToLocation()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }

        public ActionResult IssueItemHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            IssueToLocationHeadViewModel model = new IssueToLocationHeadViewModel();

            if (id != null)
            {
                // model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }

            return PartialView("_IssueItemHead", model);
        }

        public ActionResult IssueItemDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (refId != null)
            {

            }

            return PartialView("_IssueItemDetail");
        }

        public void SearchWarehouseItem(int? ItemId)
        {
            var item = VendorServices.GetItemStockWareHouseByItemId(ItemId ?? 0);
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            List<ItemStockViewModel> sessionItems = Session[WebUtil.IssueToLocation] as List<ItemStockViewModel>;

            if (sessionItems?.Count > 0)
            {
                sessionItems.Add(item);
            }
            else
            {
                items.Add(item);
                Session[WebUtil.IssueToLocation] = items;
            }
        }

        public void UpdateIssueItem(int ItemId, float Quantity)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.IssueToLocation] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    if (searchItem.BalanceQuantity >= Quantity && Quantity > 0)
                    {
                        searchItem.xQuantity = Math.Round(Quantity, 2);
                        var xremaining = Math.Round(searchItem.BalanceQuantity, 2) - searchItem.xQuantity;
                        searchItem.xRemainingQuantity = Math.Round(Convert.ToDouble(xremaining) , 2); 

                    }
                    Session[WebUtil.IssueToLocation] = sessionItems;
                }
            }
        }

        public void RemoveIssueItem(int ItemId)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.IssueToLocation] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    sessionItems.Remove(searchItem);
                    Session[WebUtil.IssueToLocation] = sessionItems;
                }
            }
        }

        [HttpPost]
        public JsonResult AddIssueToLocation(IssueToLocationHeadViewModel model)
        {
            ResponseDto responseDto = new ResponseDto();
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    responseDto = VendorServices.AddIssueToLocation(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (responseDto.Status==true)
                {
                    message = "Success";

                    Session[WebUtil.IssueToLocation] = null;
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

            return Json(responseDto);
        }

        public ActionResult AllIssueItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("AllIssueItems");
        }
        public ActionResult GetAllIssueItems()
        {
            List<IssueToLocationHeadViewModel> model = VendorServices.GetIssueToLocationHeads();
            return PartialView("_GetAllIssueItems", model);
        }

        public ActionResult IssueItemsVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["IssueVoucherID"] = Id;
            }

            return View("IssueItemsVoucherSummary");
        }

        public ActionResult GetIssueItemsVoucherSummary()
        {
            IssueToLocationHeadViewModel model = new IssueToLocationHeadViewModel();

            if (TempData["IssueVoucherID"] != null)
            {
                int Id = (int)TempData["IssueVoucherID"];
                model = VendorServices.GetIssueToLocationHeadById(Id);
                if (model != null)
                {
                    model.IssueToLocationDetails = VendorServices.GetIssueToLocationDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetIssueItemsVoucherSummary", model);
        }

        [HttpPost]
        public JsonResult DeleteIssueItemsToLocationRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteIssueToLocation(id);
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

        #region ReturnToWarehouse
        public ActionResult ReturnToWarehouse()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult ReturnToWarehouseHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ReturnToWarehouseHeadViewModel model = new ReturnToWarehouseHeadViewModel();
            if (id != null)
            {
                // model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }
            return PartialView("_ReturnToWarehouseHead", model);
        }

        public ActionResult ReturnToWarehouseDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return PartialView("_ReturnToWarehouseDetail");
        }

        public void SearchReturnToWarehouseItem(int? ItemId)
        {
            var item = VendorServices.GetItemKichenByItemId(ItemId ?? 0);
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToWarehouse] as List<ItemStockViewModel>;
            if (sessionItems?.Count > 0)
            {
                sessionItems.Add(item);
            }
            else
            {
                items.Add(item);
                Session[WebUtil.ReturnToWarehouse] = items;
            }
        }

        public void UpdateReturnToWarehouse(int ItemId, float Quantity)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToWarehouse] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    if (searchItem.kitchenblan >= Quantity && Quantity > 0)
                    {
                        
                        searchItem.xQuantity = Math.Round(Quantity, 2);
                        var xremaining = Math.Round(searchItem.kitchenblan, 2) - searchItem.xQuantity;
                        searchItem.xRemainingQuantity = Math.Round(Convert.ToDouble(xremaining), 2);
                    }
                    Session[WebUtil.ReturnToWarehouse] = sessionItems;
                }
            }
        }
        public void RemoveReturnToWarehouse(int ItemId)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToWarehouse] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    sessionItems.Remove(searchItem);
                    Session[WebUtil.ReturnToWarehouse] = sessionItems;
                }
            }
        }
        [HttpPost]
        public JsonResult AddReturnToWarehouse(ReturnToWarehouseHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    add = VendorServices.AddReturnToWarehouse(model);
                }
                if (add)
                {
                    message = "Success";

                    Session[WebUtil.ReturnToWarehouse] = null;
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
        public ActionResult AllReturnItemsToWarehouse()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("AllReturnItemsToWarehouse");
        }
        public ActionResult GetAllReturnItemsToWarehouse()
        {
            List<ReturnToWarehouseHeadViewModel> model = VendorServices.GetReturnToWarehouseHeads();
            return PartialView("_GetAllReturnItemsToWarehouse", model);
        }
        public ActionResult ReturnToWarehouseVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["IssueVoucherID"] = Id;
            }

            return View("ReturnToWarehouseVoucherSummary");
        }
        public ActionResult GetReturnToWarehouseVoucherSummary()
        {
            ReturnToWarehouseHeadViewModel model = new ReturnToWarehouseHeadViewModel();

            if (TempData["IssueVoucherID"] != null)
            {
                int Id = (int)TempData["IssueVoucherID"];
                model = VendorServices.GetReturnToWarehouseHeadById(Id);
                if (model != null)
                {
                    model.ReturnToWarehouseDetails = VendorServices.GetReturnToWarehouseDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetReturnToWarehouseVoucherSummary", model);
        }
        [HttpPost]
        public JsonResult DeleteReturnToWarehouseRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteReturnToWarehouse(id);
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

        #region ReturnToVendor
        public ActionResult ReturnToVendor()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult ReturnToVendorHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ReturnToVendorHeadViewModel model = new ReturnToVendorHeadViewModel();

            if (id != null)
            {
                // model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }

            return PartialView("_ReturnToVendorHead", model);
        }
        public ActionResult ReturnToVendorDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (refId != null)
            {

            }

            return PartialView("_ReturnToVendorDetail");
        }
        public void SearchReturnToVendorItem(int? ItemId)
        {
            var item = VendorServices.GetItemStockByItemId(ItemId ?? 0);
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToVendor] as List<ItemStockViewModel>;
            if (sessionItems?.Count > 0)
            {
                sessionItems.Add(item);
            }
            else
            {
                items.Add(item);
                Session[WebUtil.ReturnToVendor] = items;
            }
        }
        public void UpdateReturnToVendor(int ItemId,  float Quantity ,float AverageRate)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToVendor] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    if (searchItem.BalanceQuantity >= Quantity && Quantity > 0)
                    {
                        //searchItem.Rate = AverageRate;
                        searchItem.Rate = Math.Round(AverageRate , 2);
                        searchItem.xQuantity = Math.Round(Quantity , 2);
                        var xremaning= Math.Round(searchItem.BalanceQuantity, 2) - searchItem.xQuantity;
                        searchItem.xRemainingQuantity = Math.Round(Convert.ToDouble (xremaning),2);
                    }
                    Session[WebUtil.ReturnToVendor] = sessionItems;
                }
            }
        }
        public void RemoveReturnToVendor(int ItemId)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ReturnToVendor] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    sessionItems.Remove(searchItem);
                    Session[WebUtil.ReturnToVendor] = sessionItems;
                }
            }
        }
        [HttpPost]
        public JsonResult AddReturnToVendor(ReturnToVendorHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    add = VendorServices.AddReturnToVendor(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (add)
                {
                    message = "Success";

                    Session[WebUtil.ReturnToVendor] = null;
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
        public ActionResult AllReturnItemsToVendor()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult GetAllReturnItemsToVendor()
        {
            List<ReturnToVendorHeadViewModel> model = VendorServices.GetReturnToVendorHeads();
            return PartialView("_GetAllReturnItemsToVendor", model);
        }

        public ActionResult ReturnToVendorVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["IssueVoucherID"] = Id;
            }

            return View("ReturnToVendorVoucherSummary");
        }

        public ActionResult GetReturnToVendorVoucherSummary()
        {
            ReturnToVendorHeadViewModel model = new ReturnToVendorHeadViewModel();

            if (TempData["IssueVoucherID"] != null)
            {
                int Id = (int)TempData["IssueVoucherID"];
                model = VendorServices.GetReturnToVendorHeadById(Id);
                if (model != null)
                {
                    model.ReturnToVendorDetails = VendorServices.GetReturnToVendorDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetReturnToVendorVoucherSummary", model);
        }

        [HttpPost]
        public JsonResult DeleteReturnToVendorRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteReturnToVendor(id);
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

        #region Wastage
        public ActionResult Wastage()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult WastageHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            WastageHeadViewModel model = new WastageHeadViewModel();

            if (id != null)
            {
                // model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }

            return PartialView("_WastageHead", model);
        }
        public ActionResult WastageDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (refId != null)
            {

            }

            return PartialView("_WastageDetail");
        }
        public void SearchWastageItem(int? ItemId)
        {
            var item = VendorServices.GetItemStockByItemId(ItemId ?? 0);
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            List<ItemStockViewModel> sessionItems = Session[WebUtil.Wastage] as List<ItemStockViewModel>;
            if (sessionItems?.Count > 0)
            {
                sessionItems.Add(item);
            }
            else
            {
                items.Add(item);
                Session[WebUtil.Wastage] = items;
            }
        }
        public void UpdateWastage(int ItemId, float Quantity)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.Wastage] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    if (searchItem.BalanceQuantity >= Quantity && Quantity > 0)
                    {

                        searchItem.xQuantity = Math.Round(Quantity, 2);
                        var xremaining = Math.Round(searchItem.BalanceQuantity, 2) - searchItem.xQuantity;

                        searchItem.xRemainingQuantity = Math.Round(Convert.ToDouble(xremaining), 2);
                    }
                    Session[WebUtil.Wastage] = sessionItems;
                }
            }
        }
        public void RemoveWastage(int ItemId)
        {
            List<ItemStockViewModel> sessionItems = Session[WebUtil.Wastage] as List<ItemStockViewModel>;
            ItemStockViewModel searchItem = null;
            if (sessionItems?.Count > 0)
            {
                searchItem = sessionItems.Where(p => p.ItemId == ItemId).FirstOrDefault();
                if (searchItem != null)
                {
                    sessionItems.Remove(searchItem);
                    Session[WebUtil.Wastage] = sessionItems;
                }
            }
        }
        [HttpPost]
        public JsonResult AddWastage(WastageHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    add = VendorServices.AddWastage(model);
                }
                if (add)
                {
                    message = "Success";

                    Session[WebUtil.Wastage] = null;
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
        public ActionResult AllWastageItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult GetAllWastageItems()
        {
            List<WastageHeadViewModel> model = VendorServices.GetWastageHeads();
            return PartialView("_GetAllWastageItems", model);
        }
        public ActionResult WastageVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["VoucherID"] = Id;
            }

            return View("WastageVoucherSummary");
        }
        public ActionResult GetWastageVoucherSummary()
        {
            WastageHeadViewModel model = new WastageHeadViewModel();

            if (TempData["VoucherID"] != null)
            {
                int Id = (int)TempData["VoucherID"];
                model = VendorServices.GetWastageHeadById(Id);
                if (model != null)
                {
                    model.WastageDetails = VendorServices.GetWastageDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetWastageVoucherSummary", model);
        }
        [HttpPost]
        public JsonResult DeleteWastageRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteWastage(id);
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

        #region ClosingInventory
        public ActionResult ClosingInventory()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult ClosingInventoryHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ClosingInventoryHeadViewModel model = new ClosingInventoryHeadViewModel();

            if (id != null)
            {
                // model = VendorServices.GetVendorToWarehouseHeadById(id ?? 0);
            }

            return PartialView("_ClosingInventoryHead", model);
        }
        public void SearchCloseItem(int? DeptmentId)
        {
            //var item = VendorServices.GetPurchaseItemById(ItemId ?? 0);
            //List<VendorToWarehouseDetailViewModel> items = new List<VendorToWarehouseDetailViewModel>();
            //List<VendorToWarehouseDetailViewModel> sessionItems = Session[WebUtil.IssueToLocation] as List<VendorToWarehouseDetailViewModel>;

            var item = VendorServices.GetAllDeptWies(DeptmentId ?? 0);
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            Session[WebUtil.ClosingInventory] = null;
            List<ItemStockViewModel> sessionItems = Session[WebUtil.ClosingInventory] as List<ItemStockViewModel>;
          
            if (sessionItems?.Count > 0)
            {
                foreach (var itemsa in item)
                {
                    sessionItems.Add(itemsa);
                }
      
            }
            else
            {
               
                foreach (var itemsss in item)
                {
                    List<ItemStockViewModel> sessionItemsss = Session[WebUtil.ClosingInventory] as List<ItemStockViewModel>;
                    if (sessionItemsss?.Count == null)
                    {
                     
                        items.Add(itemsss);
                        Session[WebUtil.ClosingInventory] = items;
                    }
                    else
                    {
                        List<ItemStockViewModel> sessionItemss = Session[WebUtil.ClosingInventory] as List<ItemStockViewModel>;
                        sessionItemss.Add(itemsss);
                    }
                }
            }
        }
        public ActionResult ClosingInventoryDetail()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

          //  ClosingInventoryDetailViewModel model = new ClosingInventoryDetailViewModel();
            //if (DeptmentId > 0){

            //    var item = VendorServices.GetAllDeptWies(DeptmentId);

            //    List<ItemStockViewModel> sessionItems = Session[WebUtil.ClosingInventory] as List<ItemStockViewModel>;

            //    foreach (var itemsss in item)
            //    {
                    
            //        sessionItems.Add(itemsss);
            //    }
            //    // Session[WebUtil.ClosingInventory]= VendorServices.GetAllDeptWies(DeptmentId);
               
            //}
            return PartialView("_ClosingInventoryDetail");
        }
        [HttpPost]
        public JsonResult AddClosingInventory(ClosingInventoryHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    model.DeptmentId = model.DeptmentId;
                    add = VendorServices.AddClosingInventory(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (add)
                {
                    message = "Success";
                    Session[WebUtil.ClosingInventory] = null;
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
        public ActionResult AllClosingInventory()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult GetAllClosingInventory()
        {
            List<ClosingInventoryHeadViewModel> model = VendorServices.GetClosingInventoryHeads();
            return PartialView("_GetAllClosingInventory", model);
        }
        public ActionResult ClosingInventoryVoucherSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["IssueVoucherID"] = Id;
            }

            return View("ClosingInventoryVoucherSummary");
        }
        public ActionResult GetClosingInventoryVoucherSummary()
        {
            ClosingInventoryHeadViewModel model = new ClosingInventoryHeadViewModel();

            if (TempData["IssueVoucherID"] != null)
            {
                int Id = (int)TempData["IssueVoucherID"];
                model = VendorServices.GetClosingInventoryHeadById(Id);
                if (model != null)
                {
                    model.ClosingInventoryDetails = VendorServices.GetClosingInventoryDetailsByHeadId(Id);
                }
            }
            return PartialView("_GetClosingInventoryVoucherSummary", model);
        }
        [HttpPost]
        public JsonResult DeleteClosingInventoryRecord(int id)
        {
            string message = string.Empty;
            try
            {
                var del = VendorServices.DeleteClosingInventory(id);
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

        #region Report
        public ActionResult CurrentStockReport()
        {
            return View();
        }
        #endregion

        #region Opening Stock
        public ActionResult OpeningStockIndex()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult OpeningStockHead(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            OpeningStockHeadViewModel model = new OpeningStockHeadViewModel();

            if (id != null)
            {
                model = VendorServices.GetOpeningStockHeadById(id ?? 0);
            }

            return PartialView("_OpeningStockHead", model);
        }

        public ActionResult OpeningStockDetail(int? refId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            if (refId != null)
            {

            }

            return PartialView("_OpeningStockDetail");
        }

        //public void SearchItem(int? ItemId)
        //{
        //    var item = ItemServices.GetItemById(ItemId ?? 0);
        //    List<ItemViewModel> items = new List<ItemViewModel>();
        //    List<ItemViewModel> sessionItems = Session[WebUtil.SearchItems] as List<ItemViewModel>;
        //    if (sessionItems?.Count > 0)
        //    {
        //        sessionItems.Add(item);
        //    }
        //    else
        //    {
        //        items.Add(item);
        //        Session[WebUtil.SearchItems] = items;
        //    }
        //}
        public void UpdateOpeningStock(int ItemId, float Price, float Quantity)
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
                        searchItem.Price = Price;
                        searchItem.Quantity = Quantity;
                    }

                    Session[WebUtil.SearchItems] = sessionItems;
                }
            }
        }

        public void DeleteOpeningStock(int ItemId)
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
        public JsonResult AddOpeningStock(OpeningStockHeadViewModel model)
        {
            string message = string.Empty;
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            try
            {
                bool add = false;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.CreatedBy = user.Id;
                    add = VendorServices.AddOpeningStock(model);
                }
                else
                {
                    // add = ItemServices.UpdateCategory(model);
                }

                if (add)
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

            return Json(message);
        }

        public ActionResult OpeningStocItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View("OpeningStocItems");
        }
        public ActionResult GetAllOpeningStockItems()
        {
            List<OpeningStockDetailViewModel> model = VendorServices.GetOpeningDetail();
            return PartialView("_GetAllOpeningStockItems", model);
        }

        public ActionResult OpeningStockSummary(int? Id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            if (Id != null)
            {
                TempData["PurchaseVoucherID"] = Id;
            }

            return View("OpeningStockSummary");
        }

        public ActionResult GetOpeningStockVoucherSummary()
        {
            OpeningStockHeadViewModel model = new OpeningStockHeadViewModel();

            if (TempData["PurchaseVoucherID"] != null)
            {
                int Id = (int)TempData["PurchaseVoucherID"];
                model = VendorServices.GetOpeningStockHeadById(Id);
                if (model != null)
                {
                    model.OpeningStockDetails = VendorServices.GetOpeningStockDetailsByHeadId(Id);
                }
            }

            return PartialView("_GetOpeningStockVoucherSummary", model);
        }
        #endregion
    }
}