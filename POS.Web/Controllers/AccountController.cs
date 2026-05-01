using POS.Utilities.MultiTenant;
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
                    // Step 1: Resolve tenant from username (with caching)
                    var tenant = TenantCache.GetTenantByUsername(login.UserName);

                    if (tenant == null || !tenant.IsActive)
                    {
                        message = "Invalid username or tenant not found";
                        return Json(message);
                    }

                    // Step 2: Set tenant context
                    TenantContext.CurrentTenant = tenant;
                    
                    // Step 3: Authenticate user against tenant database
                    var password = Utility.Encrypt(login.Password);
                    var user = UserServices.UserLogin(login.UserName, password);

                    if (user != null && user.IsActive)
                    {
                        // Store in session
                        Session[WebUtil.CURRENT_USER] = user;
                        Session["TenantId"] = tenant.TenantId;
                        Session["TenantName"] = tenant.TenantName;
                        
                        var userRights = UserServices.GetAllUserRightsByUserId(user.Id);
                        Session[WebUtil.CurrentUserRights] = userRights;
                        
                        message = "Success";
                    }
                    else
                    {
                        // Clear tenant context on failed authentication
                        TenantContext.Clear();
                        Session.Remove("TenantId");
                        Session.Remove("TenantName");
                        message = "Invalid credentials";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
                TenantContext.Clear();
                ex.Message.ToString();
            }
            return Json(message);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            TenantContext.Clear(); // Clear tenant context
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