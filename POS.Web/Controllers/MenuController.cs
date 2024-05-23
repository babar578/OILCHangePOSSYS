using PagedList;
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
    public class MenuController : Controller
    {
        #region Item
        public ActionResult Items()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            var items = ItemServices.GetAllItems(false);
            return View("Items", items);
        }

        [HttpGet]
        public ActionResult Item(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ItemViewModel model = new ItemViewModel();
            if (id != null)
            {
                model = ItemServices.GetItemById(id ?? 0);
            }

            return View("_Item", model);
        }


        [HttpGet]
        public ActionResult GetSubCategoryByCategory(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var model = new ItemViewModel()
            {
                SubCategories = ItemServices.GetSubCategoryByCategory(id ?? 0),
            };
            return PartialView("_GetSubCategoryByCategory", model);
        }

        [HttpPost]
        public JsonResult AddItem(ItemViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.ModifyDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddItem(model);
                }
                else
                {
                    add = ItemServices.UpdateItem(model);
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

        [HttpGet]
        public ActionResult ShowItem(int id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var model = ItemServices.GetItemById(id);
            if (model != null)
                return PartialView("_ShowItem", model);
            return View();
        }

        [HttpPost]
        public JsonResult DeleteItem(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteItem(id);
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

        #region Category

        public ActionResult Categories()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var categories = ItemServices.GetAllCategories(false);
            return View("Categories", categories);
        }

        [HttpGet]
        public ActionResult Category(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            CategoryViewModel model = new CategoryViewModel();

            if (id != null)
            {
                model = ItemServices.GetCategoryById(id ?? 0);
            }

            return View("_Category", model);
        }

        [HttpPost]
        public JsonResult AddCategory(CategoryViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddCategory(model);
                }
                else
                {
                    add = ItemServices.UpdateCategory(model);
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
        public JsonResult DeleteCategory(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteCategory(id);
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

        #region SubCategory

        public ActionResult SubCategories()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var categories = ItemServices.GetAllSubCategories();
            return View("SubCategories", categories);
        }

        [HttpGet]
        public ActionResult SubCategory(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            SubCategoryViewModel model = new SubCategoryViewModel();

            if (id != null)
            {
                model = ItemServices.GetSubCategoryById(id ?? 0);
            }
            return View("_SubCategory", model);
        }

        [HttpPost]
        public JsonResult AddSubCategory(SubCategoryViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddSubCategory(model);
                }
                else
                {
                    add = ItemServices.UpdateSubCategory(model);
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
        public JsonResult DeleteSubCategory(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteSubCategory(id);
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

        #region Unit

        public ActionResult Units()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var units = ItemServices.GetAllUnits();
            return View("Units", units);
        }

        [HttpGet]
        public ActionResult Unit(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            UnitViewModel model = new UnitViewModel();

            if (id != null)
            {
                model = ItemServices.GetUnitById(id ?? 0);
            }

            return View("_Unit", model);
        }

        [HttpPost]
        public JsonResult AddUnit(UnitViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddUnit(model);
                }
                else
                {
                    add = ItemServices.UpdateUnit(model);
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
        public JsonResult DeleteUnit(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteUnit(id);
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

        #region Floor

        public ActionResult Floors()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var Floors = ItemServices.GetAllFloors();
            return View("Floors", Floors);
        }

        [HttpGet]
        public ActionResult Floor(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            FloorViewModel model = new FloorViewModel();

            if (id != null)
            {
                model = ItemServices.GetFloorById(id ?? 0);
            }

            return View("_Floor", model);
        }

        [HttpPost]
        public JsonResult AddFloor(FloorViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddFloor(model);
                }
                else
                {
                    add = ItemServices.UpdateFloor(model);
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
        public JsonResult DeleteFloor(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteFloor(id);
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

        #region FloorTable

        public ActionResult Tables(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var tables = ItemServices.GetAllFloorTablesByFloorId(id ?? 0);
            return View("Tables", tables);
        }


        public ActionResult FloorTables()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var FloorTables = ItemServices.GetAllFloorTables();
            return View("FloorTables", FloorTables);
        }

        [HttpGet]
        public ActionResult FloorTable(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            FloorTableViewModel model = new FloorTableViewModel();

            if (id != null)
            {
                model = ItemServices.GetFloorTableById(id ?? 0);
            }

            return View("_FloorTable", model);
        }

        [HttpPost]
        public JsonResult AddFloorTable(FloorTableViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddFloorTable(model);
                }
                else
                {
                    add = ItemServices.UpdateFloorTable(model);
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
        public JsonResult DeleteFloorTable(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteFloorTable(id);
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

        #region Item Group


        public ActionResult ItemGroups()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }

        public ActionResult GetAllItemGroups()
        {
            var itemGroups = ItemServices.GetAllItemGroups();
            return PartialView("_GetAllItemGroups", itemGroups);
        }

        [HttpGet]
        public ActionResult ItemGroup(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            ItemGroupViewModel model = new ItemGroupViewModel();

            if (id != null)
            {
                model = ItemServices.GetItemGroupById(id ?? 0);
            }

            return View("_ItemGroup", model);
        }

        [HttpPost]
        public JsonResult AddItemGroup(ItemGroupViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.IsActive = true;
                    add = ItemServices.AddItemGroup(model);
                }
                else
                {
                    add = ItemServices.UpdateItemGroup(model);
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
        public JsonResult DeleteItemGroup(int id)
        {
            string message = string.Empty;
            try
            {
                var del = ItemServices.DeleteItemGroup(id);
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


        #region Unit

        public ActionResult Companys()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var Companys = ItemServices.GetCompanyById();
            return View("Companys", Companys);
        }

        [HttpGet]
        public ActionResult Company(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            CompanyViewModel model = new CompanyViewModel();

            if (id != null)
            {
                model = ItemServices.GetCompanyById();
            }

            return View("_Company", model);
        }

        [HttpPost]
        public JsonResult AddCompany(CompanyViewModel model)
        {
            string message = string.Empty;
            try
            {


                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = ItemServices.AddComapny(model);
                }
                else
                {
                    //add = ItemServices.UpdateUnit(model);
                }

                if (1==1)
                {
                    message = "Success";


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