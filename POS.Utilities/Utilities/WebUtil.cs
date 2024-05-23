using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Utilities.Utilities
{
    public static class WebUtil
    {
        public static readonly string CURRENT_USER = "CurrentUser";
        public static readonly string MY_COOKIE = "Info";
        public static readonly string VIEW_NAME = "ViewName";
        public static readonly string CurrentItemsInOrder = "CurrentItemsInOrder";

        public static readonly string CurrentOrderCookies = "OrderInfo";
        public static readonly string CurrentItemsCookies = "CurrentItemsCookies";

        public static readonly string UpdateOrderCookies = "UpdateOrderInfo";
        public static readonly string UpdateItemsCookies = "UpdateItemsCookies";
        public static readonly string CurrentTable = "CurrentTable";
        public static readonly string CurrentOrder = "CurrentOrder";

        public static readonly string SearchItems = "SearchItems";
        public static readonly string Quotation = "Quotation";
        public static readonly string IssueToLocation = "IssueToLocation";
        public static readonly string ReturnToWarehouse = "ReturnToWarehouse";
        public static readonly string ReturnToVendor = "ReturnToVendor";
        public static readonly string Wastage = "Wastage";
        public static readonly string ClosingInventory = "ClosingInventory";
        public static readonly string REPORT_DATA = "ReportData";
        public static readonly string TITLE = "Title";

        public static readonly string CurrentUserRights = "CurrentUserRights";

        public static string GetInvoiceNumber()
        {
            Random random = new Random();
            int r = random.Next(1, 10000);
            string strNum = r.ToString("D4");

            return $"{DateTime.Today.ToString("yyyy-MMdd")}{strNum}";
        }
    }
}