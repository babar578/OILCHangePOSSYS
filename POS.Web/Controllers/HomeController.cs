using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Data.SqlClient;
using Newtonsoft.Json;
using POS.Utilities;
using POS.Utilities.MultiTenant;
using POS.Utilities.Services;
using POS.Utilities.Utilities;
using POS.Utilities.ViewModel;
using POS.Database.DatabaseModel;

namespace POS.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            return View();
        }

        public ActionResult GetUserDashboard(string filter = "Today")
        {
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;

            if (filter == "Yesterday")
            {
                fromDate = DateTime.Now.Date.AddDays(-1);
                toDate = DateTime.Now.Date.AddDays(-1);
            }
            else if (filter == "Last 7 Days")
            {
                fromDate = DateTime.Now.Date.AddDays(-6);
                toDate = DateTime.Now.Date;
            }
            else if (filter == "Last 30 Days")
            {
                fromDate = DateTime.Now.Date.AddDays(-29);
                toDate = DateTime.Now.Date;
            }

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Filter = filter;

            return PartialView("_GetUserDashboard");
        }

        #region POS Panel
        public ActionResult POS(int? id, int? OrderId = null, bool? IsUpdateMode = null )
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            UserViewModel model = new UserViewModel();
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            OrderViewModel order = new OrderViewModel();
            List<OrderItemViewModel> orderItems = new List<OrderItemViewModel>();
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();

            if (id != null)
            {
                Session["CurrentTableId"] = id;
            }
            else
            {
                Session.Remove("CurrentTableId");
                Session.Remove("IsUpdateMode");
                Session.Remove("IsPayment");
            }
            if (OrderId != null && IsUpdateMode.HasValue)
            {
                order = OrderServices.GetOrderById(OrderId ?? 0);
                if (order != null)
                {
                    Session["IsUpdateMode"] = IsUpdateMode;
                    Session["IsPayment"] = order.IsPayment;
                    CartUtility.AddCurrentOrder(order);
                    orderItems = OrderServices.GetOrderItemsByOrderId(order.Id);
                    if (orderItems?.Count > 0)
                    {
                        cartItems = orderItems.Select(p => (CartItemViewModel)p).ToList();
                        CartUtility.LoadItemIntoCartWithUpdateMode(cartItems, id);
                    }
                }
            }

            return View("POS",model);
        }

        public ActionResult GetCarHistory(int? itemId)
        {

            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            UserViewModel modell = new UserViewModel();
            List<GetHistoryCarVoucherViewModel> gethistory = new List<GetHistoryCarVoucherViewModel>();


            if (user == null)
            {
                return RedirectToAction("Login", new { Controller = "Account" });
            }




            if (itemId > 0)
            {
                modell.getHistoryCars = ReportServices.GetCarHistoryreportReport(Convert.ToInt32(itemId)).ToList();

            }


            return  View("GetCarHistory", modell);
        }


        [HttpPost]
        public ActionResult GetCarHistorybyId(int? itemId)
        {

            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            UserViewModel modell = new UserViewModel();
            List<GetHistoryCarVoucherViewModel> gethistory = new List<GetHistoryCarVoucherViewModel>();
               

            if (user == null)
            {
                return RedirectToAction("Login", new { Controller = "Account" });
            }




            if (itemId > 0)
            {
                modell.getHistoryCars = ReportServices.GetCarHistoryreportReport(Convert.ToInt32(itemId)).ToList();
                foreach (var item in modell.getHistoryCars)
                {
                    item.DateDate =  (item.CreationDate).ToString(WebConfigSettings.DateTimeFormat);
                   
                }


            }

            return Json(modell, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Categories()
        {
            return PartialView("_Categories");
        }

        public ActionResult CategoryDetails(int? CategoryId)
        {
            List<ItemStockViewModel> items = new List<ItemStockViewModel>();
            if (CategoryId.HasValue)
            {
                items = VendorServices.GetItemStockbyCategoriesID(CategoryId ?? 0);
            }
            return PartialView("_CategoryDetails", items);
        }

        /// <summary>
        /// Settings page with Company, Users, Rights, and Configuration
        /// </summary>
        public ActionResult Settings()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            
            return View();
        }

        /// <summary>
        /// Display low stock items page
        /// </summary>
        public ActionResult LowStockItems()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });
            
            var lowStockItems = VendorServices.GetLowStockItems();
            return View(lowStockItems);
        }

        /// <summary>
        /// Check if category has low stock (zero or negative balance)
        /// </summary>
        [HttpPost]
        public JsonResult CheckCategoryStock(int? CategoryId)
        {
            try
            {
                if (!CategoryId.HasValue)
                {
                    return Json(new { hasLowStock = false });
                }

                // Get all items for this category and check their stock balance
                var items = VendorServices.GetItemStockbyCategoriesID(CategoryId ?? 0);
                
                // Check if any item has zero or negative balance
                // BalanceQuantity <= 0 means low/out of stock
                bool hasLowStock = items != null && items.Any(item => item.BalanceQuantity <= 0);
                
                return Json(new { hasLowStock = hasLowStock });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking category stock: {ex.Message}");
                // Return false on error so user can continue
                return Json(new { hasLowStock = false });
            }
        }

        [HttpPost]
        public JsonResult IsHoldTable(int id)
        {
            string message = string.Empty;
            try
            {
                var hold = ItemServices.IsHoldTable(id);
                if (hold)
                {
                    string table = WebUtil.CurrentTable + id;
                    HttpCookie tableCookie = new HttpCookie(table)
                    {
                        Value = "",
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(tableCookie);
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
        public JsonResult IsReleaseTable(int id)
        {
            string message = string.Empty;
            try
            {
                var release = ItemServices.IsReleaseTable(id);
                if (release)
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

        public ActionResult SideMenu()
        {
            return PartialView("_SideMenu");
        }

        #endregion
        #region AddToCart Working
        public ActionResult CartItems()
        {
            return PartialView("_CartItems");
        }



        public void SearchBarcodeItem(string Barcode)
        {
            if (Barcode != "")
            {
                var item = ItemServices.GetItemcurrentbarcodeById(Barcode);

                List<ItemViewModel> items = new List<ItemViewModel>();
                List<ItemViewModel> sessionItems = Session[WebUtil.SearchItems] as List<ItemViewModel>;
                if (sessionItems?.Count > 0)
                {
                    sessionItems.Add(item);
                   // itemId, qty, price, itemName, taxAmount, taxPercentage, currentTableId, departmentId
                    CartItemViewModel model =new CartItemViewModel();
                    model.ItemId = item.Id;
                    model.Quantity = 1;
                    model.Price = item.Price;
                    model.ItemName=item.Name;
                    model.DepartmentId= item.DepartmentId;


                    AddItemIntoCart(model);
                }
                else
                {
                    items.Add(item);
                    Session[WebUtil.SearchItems] = items;
                }
            }

        }


        [HttpPost]
        public JsonResult AddItemIntoCart(CartItemViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add = CartUtility.AddItemIntoCart(model);

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
        public JsonResult CurrentItemRemarks(string id, int? tableId = null, string remarks = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.CurrentItemRemarks(id, tableId, remarks);
                }
                else
                {
                    add = CartUtility.CurrentItemRemarks(id, null, remarks);
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
        public JsonResult RemoveItemFromCart(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.RemoveItemFromCart(id, tableId);
                }
                else
                {
                    add = CartUtility.RemoveItemFromCart(id);
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
        public JsonResult VoidItemFromCart(string id, int? tableId = null, double? Price = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.VoidItemFromCart(id, tableId);
                }
                else
                {
                    add = CartUtility.VoidItemFromCart(id);
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
        public JsonResult ComplimentaryItemFromCart(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.ComplimentaryItemFromCart(id, tableId);
                }
                else
                {
                    add = CartUtility.ComplimentaryItemFromCart(id);
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
        public JsonResult ReleaseVoidItemFromCart(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.ReleaseVoidItemFromCart(id, tableId);
                }
                else
                {
                    add = CartUtility.ReleaseVoidItemFromCart(id);
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
        public JsonResult ReleaseComplimentaryItemFromCart(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.ReleaseComplimentaryItemFromCart(id, tableId);
                }
                else
                {
                    add = CartUtility.ReleaseComplimentaryItemFromCart(id);
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
        //PointQuantity
        [HttpPost]
        public JsonResult PointQuantity(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;

                if (tableId != null)
                {
                    add = CartUtility.PointQuantity(id, tableId);
                }
                else
                {
                    add = CartUtility.PointQuantity(id);
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
        public JsonResult PlusQuantity(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;

                if (tableId != null)
                {
                    add = CartUtility.PlusQuantity(id, tableId);
                }
                else
                {
                    add = CartUtility.PlusQuantity(id);
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
        public JsonResult MinusQuantity(string id, int? tableId = null)
        {
            string message = string.Empty;
            try
            {
                bool add;
                if (tableId != null)
                {
                    add = CartUtility.MinusQuantity(id, tableId);
                }
                else
                {
                    add = CartUtility.MinusQuantity(id);
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
        public JsonResult UpdateItemQty(string id, double qty)
        {
            string message = string.Empty;
            try
            {
                bool add = CartUtility.UpdateItemQty(id, qty);
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
        #region CartButtons
        public ActionResult Payment()
        {
            return PartialView("_Payment");
        }

        public ActionResult PaymentByCash()
        {
            return PartialView("_PaymentByCash");
        }

        public ActionResult GST()
        {
            return PartialView("_GST");
        }


        //aliment


        public ActionResult AlignmentAmount()
        {
            return PartialView("_AlignmentAmount");
        }

        public ActionResult WheelBalanceAmount()
        {
            return PartialView("_wheelBalanceAmount");
        }


        public ActionResult NitrogenGas()
        {
            return PartialView("_NitrogenGas");
        }

        public ActionResult TPMS()
        {
            return PartialView("_TPMS");
        }


        /// <summary>
        /// add new button in oil carth 
        /// </summary>
        /// <returns></returns>



        public ActionResult Discount()
        {
            return PartialView("_Discount");
        }

        public ActionResult DiscountPer()
        {
            return PartialView("_DiscountPer");
        }

        public ActionResult Tip()
        {
            return PartialView("_Tip");
        }

        public ActionResult ServiceCharges()
        {
            return PartialView("_ServiceCharges");
        }
        #endregion
        #region Order Working
        public JsonResult CreateTicket(OrderViewModel model)
        {
            string message = string.Empty;
            try
            {

                bool add = CartUtility.AddCurrentOrder(model);

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
        public JsonResult AddOrder(int? tableId)
        {
            string message = string.Empty;
            bool add = false;
            bool IsUpdate = false;



            if (Session["IsUpdateMode"] != null)
            {
                IsUpdate = (bool)Session["IsUpdateMode"];
            }
            OrderViewModel order = new OrderViewModel();
            try
            {
             
                List<CartItemViewModel> CartItems = new List<CartItemViewModel>();

                if (tableId != null)
                {
                    order = CartUtility.CurrentOrder(tableId);
                    CartItems = CartUtility.GetCartItems(tableId);
                }
                else
                {
                    order = CartUtility.CurrentOrder();
                    CartItems = CartUtility.GetCartItems();

                }
                if (
                    order.IsPayment == false)
                    order.IsUpdateMode = true;
                else
                    order.IsUpdateMode = false;

                if (order.Id == 0)
                {
                    int inviceNo = 0;
                    //order.PaymentTypeId = 1;
                    var d = OrderServices.GetOrder();

                    if(d != null)
                    {
                         inviceNo = d.Id + 1;
                    }
                    else
                    {
                        inviceNo = 1;

                    }

                    order.InvoiceNumber = Convert.ToString(inviceNo);
              
                    order.CreationDate = DateTime.Now;
                    order.ModifyDate = DateTime.Now;
                    order = OrderServices.AddOrder(order, CartItems);
                    add = order.returnValue;
                }
                else
                {
                    add = OrderServices.UpdateOrder(order, CartItems);
                }
                if (add)
                {
                   order.massage = "Success";
                    //expire cookie
                    if (tableId != null)
                    {
                        string currentTable = WebUtil.CurrentTable + tableId;
                        string currentOrder = WebUtil.CurrentOrder + tableId;
                        if (Request.Cookies[currentTable] != null)
                        {
                            Response.Cookies[currentTable].Expires = DateTime.Now.AddDays(-1);
                        }
                        if (Request.Cookies[currentOrder] != null)
                        {
                            Response.Cookies[currentOrder].Expires = DateTime.Now.AddDays(-1);
                        }
                    }
                    else
                    {
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
                    }
                }
                else
                {
                    order.massage = "Error";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Order_Void_Or_Complimentary(int? id, bool IsVoid, bool IsComplimentary, bool IsPayment, bool IsUpdateMode, string Reason = null)
        {
            string message = string.Empty;
            try
            {
                var add = OrderServices.Add_Order_Void_Or_Complimentary(id, IsVoid, IsComplimentary, IsPayment, IsUpdateMode, Reason);

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
        public JsonResult CurrentOrderReason(int id, string reason = null)
        {
            string message = string.Empty;
            try
            {
                bool add = OrderServices.AddOrderWithReason(id, reason);

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
        public ActionResult AllOrders()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            return View();
        }
        public ActionResult GetAllOrders()
        {
            var model = OrderServices.GetAllOrders();


            foreach (var item in model)
            {
                var models = OrderServices.GetCustomerById(Convert.ToInt32(item.NoOfGuest));
                if (models != null && models.CarNumber != null)
                {
                    String NoOfGuest = models.CarNumber;

                    item.CarNo = NoOfGuest;
                }
               

            }

            return PartialView("_GetAllOrders", model);
        }
        [HttpPost]
        public JsonResult CloseOrder(int id)
        {
            string message = string.Empty;
            try
            {
                var close = OrderServices.CloseOrder(id);
                if (close)
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
        public JsonResult OpenOrder(int id)
        {
            string message = string.Empty;
            try
            {
                var open = OrderServices.OpenOrder(id);
                if (open)
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
        public ActionResult OpenOrders()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            return View();
        }
        public ActionResult GetOpenOrders()
        {
            var model = OrderServices.GetOrdersWithOutPayment();
            foreach (var item in model)
            {
                var models = OrderServices.GetCustomerById(Convert.ToInt32(item.NoOfGuest));
               String  NoOfGuest = models.CarNumber;

                item.CarNo = NoOfGuest;

            }

            return PartialView("_GetOpenOrders", model);
        }
        public ActionResult DraftOrders()
        {
            var model = OrderServices.GetOrdersWithOutPayment();
            return PartialView(model);
        }
         class QuickMessage {
        private String MSISDN;
        private String PASSWORD;
        private String abc;
   public     QuickMessage(String msisdn, String password)
        {
            this.MSISDN = msisdn;
            this.PASSWORD = password;
        }
            public String getSessionId()
        {
            String url = "https://telenorcsms.com.pk:27677/corporate_sms2/api/auth.jsp?msisdn=" +
           MSISDN + "&password=" + PASSWORD;
            return sendRequest(url);
        }

            public String sendQuickMessage(String sessionId, String messageText, String to, String mask)
        {
            String url =
           "https://telenorcsms.com.pk:27677/corporate_sms2/api/sendsms.jsp?session_id=" + sessionId +
           "&text=" + messageText + "&to=" + to;
            if (mask != null)
            {
                url = url += "&mask=" + mask;
            }
            return sendRequest(url);
        }
        private String sendRequest(String url)
        {
            String response = null;
            try
            {
                var client = new WebClient();
                response = client.DownloadString(url);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(response);
                XmlNodeList responseType = xmldoc.GetElementsByTagName("response");
                XmlNodeList data = xmldoc.GetElementsByTagName("data");
                if (responseType.Equals("Error"))
                {
                    return null;
                }
                response = data[0].InnerText;
                return response;
            }

        catch (Exception e)
 {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        }
        public ActionResult PrintInvoice(int? id )
        {
            OrderViewModel order = new OrderViewModel();
            if (id >0)
            {
                order = OrderServices.GetOrderById(id ?? 0);
                if (order != null)
                {
                    order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);
                    var CustomerName = VendorServices.GetCustomerById(order.NoOfGuest ?? 0);
                    order.CustomerName = CustomerName.CustomerName;
                    order.CarNo = CustomerName.CarNumber;
                    order.Reading = CustomerName.Reading;
                    order.CreationDate =  DateTime.Now;

                    // Reading sticker helpers (computed at print-time)
                    if (double.TryParse(order.Reading, out var lastReading))
                        order.LastReading = lastReading;
                    order.CurrentReading = order.Tip;
                    order.NextReading = order.LastReading + order.CurrentReading;
                    order.NextReadingDate = DateTime.Now.Date.AddDays(30);

                    // Try to pick an "oil" item name; fallback to first item
                    var oilItem = order.OrderItems?
                        .FirstOrDefault(x => (x.ItemName ?? "").ToLower().Contains("oil"))
                        ?? order.OrderItems?.FirstOrDefault();
                    order.OilItemName = oilItem?.ItemName;


                    // Check if SMS is enabled and send SMS dynamically
                    var smsSettings = MessagingSettingsService.GetSMSSettings();
                    if (smsSettings != null && smsSettings.IsEnabled && !string.IsNullOrEmpty(smsSettings.Username) && !string.IsNullOrEmpty(smsSettings.Password))
                    {
                        //Set message text which you want to send
                        String messageText = $"Hello {CustomerName.CustomerName},\r\nDate:{DateTime.Now}.\r\nCar No:{CustomerName.CarNumber}\r\n,Current Reading:{order.Tip}.\r\nThanks for choosing SHAHZAD OIL STORE CENTRAL PARK.";
                        //String messageText = "📜 یوم اقبال مبارک ہو!\r\n\r\n📚 \"خودی کر بولنا، کی ہار قسمت کے ہے،\r\n\"خود رب سے پوچھو، بتاؤ تمہارا راز کیا ہے؟\"\r\n\r\n\r\nشہزاد آئل سٹور سنٹرل پارک لاہور";
                        String to = CustomerName.Mobile;
                        //Get mask from settings
                        String mask = smsSettings.Mask ?? "Shahzad Oil";
                        //Get credentials from database settings
                        String userName = smsSettings.Username;
                        String password = smsSettings.Password;
                        
                        QuickMessage obj = new QuickMessage(userName, password);
                        String sessionId = obj.getSessionId();
                        if (sessionId != null && !string.IsNullOrEmpty(to))
                        {
                            //Un Coment After Use Metro Only Use For Moshin Oil 
                            String messageIds = obj.sendQuickMessage(sessionId, messageText, to, mask);
                          
                        }
                    }
                }

                return PartialView("_PrintInvoice", order);
            }
            return RedirectToAction("Login", new { area = "", Controller = "Account" });
        }

        [HttpGet]
        public ActionResult PrintReadingSticker(int id)
        {
            var order = OrderServices.GetOrderById(id);
            if (order == null)
                return new HttpNotFoundResult();

            order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);

            var customer = VendorServices.GetCustomerById(order.NoOfGuest ?? 0);
            if (customer != null)
            {
                order.CustomerName = customer.CustomerName;
                order.CarNo = customer.CarNumber;
                order.Reading = customer.Reading;
            }

            if (double.TryParse(order.Reading, out var lastReading))
                order.LastReading = lastReading;
            order.CurrentReading = order.Tip;
            order.NextReading = order.LastReading + order.CurrentReading;
            order.NextReadingDate = DateTime.Now.Date.AddDays(30);

            var oilItem = order.OrderItems?
                .FirstOrDefault(x => (x.ItemName ?? "").ToLower().Contains("oil"))
                ?? order.OrderItems?.FirstOrDefault();
            order.OilItemName = oilItem?.ItemName;

            order.CreationDate = DateTime.Now;

            return PartialView("_ReadingSticker", order);
        }

        [HttpGet]
        public ActionResult PrintLatestReadingSticker()
        {
            var order = OrderServices.GetLatestPaidOrder();
            if (order == null || order.Id <= 0)
                return new HttpNotFoundResult("No paid invoice found to print sticker.");

            order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);

            var customer = VendorServices.GetCustomerById(order.NoOfGuest ?? 0);
            if (customer != null)
            {
                order.CustomerName = customer.CustomerName;
                order.CarNo = customer.CarNumber;
                order.Reading = customer.Reading;
            }

            if (double.TryParse(order.Reading, out var lastReading))
                order.LastReading = lastReading;
            order.CurrentReading = order.Tip;
            order.NextReading = order.LastReading + order.CurrentReading;
            order.NextReadingDate = DateTime.Now.Date.AddDays(30);

            var oilItem = order.OrderItems?
                .FirstOrDefault(x => (x.ItemName ?? "").ToLower().Contains("oil"))
                ?? order.OrderItems?.FirstOrDefault();
            order.OilItemName = oilItem?.ItemName;

            order.CreationDate = DateTime.Now;

            return PartialView("_ReadingSticker", order);
        }
     
        [HttpPost]
        public JsonResult PrintAllOrderDepartments(int id)
        {
            string message = string.Empty;
            try
            {
                var order = OrderServices.GetOrderById(id);
                var print = InvoicePrinter.PrintToSmallPrinter(order);
                if (print)
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
        public JsonResult PrintKitchenOrder(int id)
        {
            string message = string.Empty;
            try
            {
                var order = OrderServices.GetOrderById(id);

                if (order != null)
                {
                    order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);
                }

                var print = InvoicePrinter.KitchenInoviceFormat(order, 1);
                if (print)
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
        public JsonResult PrintBarOrder(int id)
        {
            string message = string.Empty;
            try
            {
                var order = OrderServices.GetOrderById(id);
                if (order != null)
                {
                    order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);
                }
                var print = InvoicePrinter.BarInoviceFormat(order, 2);
                if (print)
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
        public JsonResult PrintDessertOrder(int id)
        {
            string message = string.Empty;
            try
            {
                var order = OrderServices.GetOrderById(id);
                if (order != null)
                {
                    order.OrderItems = OrderServices.GetOrderItemsByOrderId(order.Id);
                }
                var print = InvoicePrinter.DessertInoviceFormat(order, 3);
                if (print)
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
        public ActionResult AddOrderFeedBack(int? orderId)
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            return PartialView("_AddOrderFeedBack");
        }

        [HttpPost]
        public JsonResult AddOrderFeedBack(OrderFeedbackViewModel model)
        {
            string message = string.Empty;
            try
            {
                bool add = OrderServices.AddOrderFeedback(model);
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

        /// <summary>
        /// Sends SMS reminders to customers for oil change service based on their last order date and number of days
        /// This method is called automatically once per day via Hangfire scheduler
        /// Updated for multi-tenancy: accepts tenantId parameter
        /// </summary>
        public static void SendOilChangeReminderSMS(int tenantId)
        {
            List<CustomerReminderViewModel> customersToRemind = null;
            
            try
            {
                // Resolve tenant
                var tenant = TenantResolver.GetTenantById(tenantId);
                if (tenant == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Tenant {tenantId} not found for SMS reminder job");
                    return;
                }

                // Set tenant context
                TenantContext.CurrentTenant = tenant;
                
                customersToRemind = new List<CustomerReminderViewModel>();

                // Execute the SQL query to get customers who need reminders today
                using (var context = MultiTenantDbContextFactory.CreateDbContext())
                {
                    string sqlQuery = @"
                        WITH LatestOrders AS (
                            SELECT 
                                o.Id, o.InvoiceNumber, CONVERT(DATE,o.CreationDate) AS CreationDate,
                                o.NoOfGuest AS CustomerID, c.CustomerName, c.CarNumber, c.Mobile,
                                CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END AS NoOfDays,
                                ROW_NUMBER() OVER (PARTITION BY o.NoOfGuest ORDER BY o.CreationDate DESC) AS rn
                            FROM Orders o
                            INNER JOIN Customer c ON c.Id = o.NoOfGuest
                            WHERE TRY_CONVERT(BIGINT, c.CNIC) IS NOT NULL
                            AND CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END > 0
                        )
                        SELECT 
                            Id, InvoiceNumber, CreationDate,
                            CustomerID, CustomerName, CarNumber, Mobile, NoOfDays,
                            CONVERT(DATE,DATEADD(DAY,NoOfDays,CreationDate)) AS ActionDate
                        FROM LatestOrders
                        WHERE rn = 1 and Mobile Like '923114343115'
                        AND CONVERT(DATE,DATEADD(DAY,NoOfDays,CreationDate)) = CONVERT(DATE,GETDATE())
                        ORDER BY CreationDate DESC";

                    customersToRemind = context.Database.SqlQuery<CustomerReminderViewModel>(sqlQuery).ToList();
                }

                // Send SMS to each customer - Dynamic SMS from database settings
                if (customersToRemind != null && customersToRemind.Count > 0)
                {
                    // Get SMS settings from database
                    var smsSettings = MessagingSettingsService.GetSMSSettings();
                    
                    // Only send SMS if enabled and credentials are configured
                    if (smsSettings != null && smsSettings.IsEnabled && 
                        !string.IsNullOrEmpty(smsSettings.Username) && !string.IsNullOrEmpty(smsSettings.Password))
                    {
                        // SMS credentials from database
                        String userName = smsSettings.Username;
                        String password = smsSettings.Password;
                        String mask = smsSettings.Mask ?? "Shahzad Oil";
                        
                        QuickMessage smsClient = new QuickMessage(userName, password);
                        String sessionId = smsClient.getSessionId();

                        if (sessionId != null)
                        {
                            foreach (var customer in customersToRemind)
                            {
                                try
                                {
                                    // Create reminder message
                                    String messageText = $"Hello {customer.CustomerName},\r\n" +
                                        $"Your car ({customer.CarNumber}) is due for oil change service.\r\n" +
                                        $"Last service date: {customer.CreationDate:dd/MM/yyyy}\r\n" +
                                        $"Please visit SHAHZAD OIL STORE CENTRAL PARK for your next service.\r\n" +
                                        $"Thank you!";

                                    // Send SMS
                                    if (!string.IsNullOrEmpty(customer.Mobile))
                                    {
                                       // String messageIds = smsClient.sendQuickMessage(sessionId, messageText, customer.Mobile, mask);
                                        // Log success (you can add logging here if needed)
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Log error for individual customer (you can add logging here)
                                    System.Diagnostics.Debug.WriteLine($"Error sending SMS to {customer.CustomerName}: {ex.Message}");
                                }
                            }
                        }
                    }
                    else
                    {
                        // SMS is disabled or not configured
                        System.Diagnostics.Debug.WriteLine("SMS is disabled or not configured. Skipping SMS reminders.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error (you can add proper logging here)
                System.Diagnostics.Debug.WriteLine($"Error in SendOilChangeReminderSMS for tenant {tenantId}: {ex.Message}");
            }
            finally
            {
                // Clear tenant context
                TenantContext.Clear();
            }
        }
        [HttpGet]
        public JsonResult GetDailySummary()
        {
            try
            {
                var result = DashboardServices.GetDailySummary(DateTime.Now);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetProfitReport(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var from = fromDate ?? DateTime.Now.Date;
                var to = toDate ?? DateTime.Now.Date;
                var result = DashboardServices.GetProfitReport(from, to);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get count of reminder messages sent today
        /// </summary>
        [HttpGet]
        public JsonResult GetTodayRemindersCount()
        {
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string sqlQuery = @"
                        WITH LatestOrders AS (
                            SELECT 
                                o.Id, o.InvoiceNumber, CONVERT(DATE,o.CreationDate) AS CreationDate,
                                o.NoOfGuest AS CustomerID, c.CustomerName, c.CarNumber, c.Mobile,
                                CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END AS NoOfDays,
                                ROW_NUMBER() OVER (PARTITION BY o.NoOfGuest ORDER BY o.CreationDate DESC) AS rn
                            FROM Orders o
                            INNER JOIN Customer c ON c.Id = o.NoOfGuest
                            WHERE TRY_CONVERT(BIGINT, c.CNIC) IS NOT NULL
                            AND CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END > 0
                        )
                        SELECT COUNT(*) as Count
                        FROM LatestOrders
                        WHERE rn = 1
                        AND CONVERT(DATE,DATEADD(DAY,NoOfDays,CreationDate)) = CONVERT(DATE,GETDATE())";

                    var count = context.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
                    return Json(count, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get list of reminder messages sent today
        /// </summary>
        [HttpGet]
        public JsonResult GetTodayReminders()
        {
            try
            {
                List<CustomerReminderViewModel> reminders = new List<CustomerReminderViewModel>();
                using (POSEntities context = new POSEntities())
                {
                    string sqlQuery = @"
                        WITH LatestOrders AS (
                            SELECT 
                                o.Id, o.InvoiceNumber, CONVERT(DATE,o.CreationDate) AS CreationDate,
                                o.NoOfGuest AS CustomerID, c.CustomerName, c.CarNumber, c.Mobile,
                                CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END AS NoOfDays,
                                ROW_NUMBER() OVER (PARTITION BY o.NoOfGuest ORDER BY o.CreationDate DESC) AS rn
                            FROM Orders o
                            INNER JOIN Customer c ON c.Id = o.NoOfGuest
                            WHERE TRY_CONVERT(BIGINT, c.CNIC) IS NOT NULL
                            AND CASE WHEN CONVERT(INT, c.CNIC) > 100 THEN 0 ELSE CONVERT(INT, c.CNIC) END > 0
                        )
                        SELECT 
                            Id, InvoiceNumber, CreationDate,
                            CustomerID, CustomerName, CarNumber, Mobile, NoOfDays,
                            CONVERT(DATE,DATEADD(DAY,NoOfDays,CreationDate)) AS ActionDate
                        FROM LatestOrders
                        WHERE rn = 1
                        AND CONVERT(DATE,DATEADD(DAY,NoOfDays,CreationDate)) = CONVERT(DATE,GETDATE())
                        ORDER BY CreationDate DESC";

                    reminders = context.Database.SqlQuery<CustomerReminderViewModel>(sqlQuery).ToList();
                }

                // Format the results for display
                var result = reminders.Select(r => new
                {
                    CustomerName = r.CustomerName,
                    CarNumber = r.CarNumber,
                    Mobile = r.Mobile,
                    Message = $"Hello {r.CustomerName}, Your car ({r.CarNumber}) is due for oil change service. Last service date: {r.CreationDate:dd/MM/yyyy}",
                    SentTime = DateTime.Now.ToString("hh:mm tt")
                }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SMS & WhatsApp Settings

        [HttpGet]
        public ActionResult GetSMSSettings()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var settings = MessagingSettingsService.GetSMSSettings();
            return PartialView("_SMSSettings", settings);
        }

        [HttpPost]
        public JsonResult SaveSMSSettings(SMSSettingsViewModel model)
        {
            string message = string.Empty;
            try
            {
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                if (user != null)
                {
                    model.ModifiedBy = user.Id;
                    if (model.Id == 0)
                    {
                        model.CreatedBy = user.Id;
                    }
                }

                bool saved = MessagingSettingsService.SaveSMSSettings(model);
                if (saved)
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
                message = "Error: " + ex.Message;
            }
            return Json(message);
        }

        [HttpPost]
        public JsonResult ToggleSMS(bool isEnabled)
        {
            string message = string.Empty;
            try
            {
                bool toggled = MessagingSettingsService.ToggleSMSEnabled(isEnabled);
                if (toggled)
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
                message = "Error: " + ex.Message;
            }
            return Json(message);
        }

        [HttpGet]
        public ActionResult GetWhatsAppSettings()
        {
            var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
            if (user == null)
                return RedirectToAction("Login", new { Controller = "Account" });

            var settings = MessagingSettingsService.GetWhatsAppSettings();
            return PartialView("_WhatsAppSettings", settings);
        }

        [HttpPost]
        public JsonResult SaveWhatsAppSettings(WhatsAppSettingsViewModel model)
        {
            string message = string.Empty;
            try
            {
                var user = Session[WebUtil.CURRENT_USER] as UserViewModel;
                if (user != null)
                {
                    model.ModifiedBy = user.Id;
                    if (model.Id == 0)
                    {
                        model.CreatedBy = user.Id;
                    }
                }

                bool saved = MessagingSettingsService.SaveWhatsAppSettings(model);
                if (saved)
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
                message = "Error: " + ex.Message;
            }
            return Json(message);
        }

        [HttpPost]
        public JsonResult ToggleWhatsApp(bool isEnabled)
        {
            string message = string.Empty;
            try
            {
                bool toggled = MessagingSettingsService.ToggleWhatsAppEnabled(isEnabled);
                if (toggled)
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
                message = "Error: " + ex.Message;
            }
            return Json(message);
        }

        #endregion
    }
}