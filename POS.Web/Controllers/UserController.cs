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
    public class UserController : Controller
    {
        // GET: User

        #region Users
        public ActionResult AllUsers()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult GetAllUsers()
        {
            var model = UserServices.GetAllUsers();
            return PartialView("_GetAllUsers", model);
        }
        [HttpGet]
        public ActionResult AddUser(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            UserViewModel model = new UserViewModel();
            if (id != null)
            {
                model = UserServices.GetUserById(id ?? 0);
            }
            return View("_AddUser", model);
        }
        [HttpPost]
        public JsonResult AddUser(UserViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.CreationDate = DateTime.Now;
                    model.IsActive = true;
                    add = UserServices.AddUser(model);
                }
                else
                {
                    add = UserServices.UpdateUser(model);
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
        public JsonResult DeleteUser(int id)
        {
            string message = string.Empty;
            try
            {
                var del = UserServices.DeleteUser(id);
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
        #region UserRight Panel
        public ActionResult UserRights()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }
        public ActionResult GetUsers()
        {
            return PartialView("_GetUsers");
        }
        public ActionResult GetUserRights(int UserId)
        {
            List<UserRightViewModel> userRights = new List<UserRightViewModel>();
            if (UserId > 0)
            {
                Session["SearchUserId"] = UserId;
                userRights = UserServices.GetAllUserRightsByUserId(UserId);
            }
            return PartialView("_GetUserRights", userRights);
        }
        [HttpPost]
        public JsonResult AddUserRights(int UserId, List<int> UserRights)
        {
            string message = string.Empty;
            try
            {
                bool add = UserServices.AddNewUserRight(UserId, UserRights);
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

        #endregion

        #region Designation
        public ActionResult Designations()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            var Designations = UserServices.GetAllDesignations();
            return View("Designations", Designations);
        }

        [HttpGet]
        public ActionResult Designation(int? id)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            DesignationViewModel model = new DesignationViewModel();
            if (id != null)
            {
                model = UserServices.GetDesignationById(id ?? 0);
            }
            return PartialView("_Designation", model);
        }
        [HttpPost]
        public JsonResult AddDesignation(DesignationViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (model.Id == 0)
                {
                    model.IsActive = true;
                    add = UserServices.AddDesignation(model);
                }
                else
                {
                    add = UserServices.UpdateDesignation(model);
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
        public JsonResult DeleteDesignation(int id)
        {
            string message = string.Empty;
            try
            {
                var del = UserServices.DeleteDesignation(id);
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
        #region ShopStatus
        [HttpPost]
        public JsonResult OpenShop()
        {
            string message = string.Empty;
            try
            {
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                ShopStatusViewModel model = new ShopStatusViewModel()
                {
                    OpenStatus = true,
                    DateOpen = DateTime.Now,
                    ShopOpenUserId = user.Id,
                };
                var add = UserServices.ShopOpenStatus(model);
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
        public JsonResult CloseShop()
        {
            string message = string.Empty;
            try
            {
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                var add = UserServices.ShopCloseStatus(user.Id);
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

        #endregion
    }
}