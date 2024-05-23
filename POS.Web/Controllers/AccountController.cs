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
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(UserViewModel login)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(login.UserName) && !string.IsNullOrEmpty(login.Password))
                {
                    var password = Utility.Encrypt(login.Password);
                    var user = UserServices.UserLogin(login.UserName, password);
                    if (user != null && user.IsActive)
                    {
                        Session.Add(WebUtil.CURRENT_USER, user);
                        var userRights = UserServices.GetAllUserRightsByUserId(user.Id);
                        Session.Add(WebUtil.CurrentUserRights, userRights);
                        message = "Success";
                    }
                    else
                    {
                        message = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Error";
                ex.Message.ToString();
            }
            return Json(message);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.Contents.RemoveAll();

            if (Request.Cookies[WebUtil.CurrentItemsCookies] != null)
            {
                Response.Cookies[WebUtil.CurrentItemsCookies].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies[WebUtil.CurrentOrderCookies] != null)
            {
                Response.Cookies[WebUtil.CurrentOrderCookies].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies[WebUtil.UpdateOrderCookies] != null)
            {
                Response.Cookies[WebUtil.UpdateOrderCookies].Expires = DateTime.Now.AddDays(-1);
            }
            if (Request.Cookies[WebUtil.UpdateItemsCookies] != null)
            {
                Response.Cookies[WebUtil.UpdateItemsCookies].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Login", new { area = "", controller = "Account" });
        }
    }
}