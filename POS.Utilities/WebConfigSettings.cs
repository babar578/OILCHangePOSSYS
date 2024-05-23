using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace POS.Utilities
{
 public static class WebConfigSettings
    {
        #region App

        public static string BaseURL => Convert.ToString(WebConfigurationManager.AppSettings["BaseURL"]);
        public static int ChatDays => Convert.ToInt32(WebConfigurationManager.AppSettings["ChatDays"]);
        public static int ActivityDays => Convert.ToInt32(WebConfigurationManager.AppSettings["ActivityDays"]);
        public static int NotificationDays => Convert.ToInt32(WebConfigurationManager.AppSettings["NotificationDays"]);
        public static string EnableNotifications => Convert.ToString(WebConfigurationManager.AppSettings["EnableNotifications"]);
        public static string EnableEmails => Convert.ToString(WebConfigurationManager.AppSettings["EnableEmails"]);

        #endregion

        #region DateTime

        public static string DateFormatCalendar => Convert.ToString(WebConfigurationManager.AppSettings["DateFormatCalendar"]);
        public static string DateFormat => Convert.ToString(WebConfigurationManager.AppSettings["DateFormat"]);
        public static string DateTimeFormat => Convert.ToString(WebConfigurationManager.AppSettings["DateTimeFormat"]);
        public static string MonthYearFormat => Convert.ToString(WebConfigurationManager.AppSettings["MonthYearFormat"]);
        public static string TimeFormat => Convert.ToString(WebConfigurationManager.AppSettings["TimeFormat"]);
        public static string DateFormatJQuery => Convert.ToString(WebConfigurationManager.AppSettings["DateFormatJQuery"]);
        public static string DateTimeFormatJQuery => Convert.ToString(WebConfigurationManager.AppSettings["DateTimeFormatJQuery"]);
        public static string MonthYearFormatJQuery => Convert.ToString(WebConfigurationManager.AppSettings["MonthYearFormatJQuery"]);
        public static string TimeFormatJQuery => Convert.ToString(WebConfigurationManager.AppSettings["TimeFormatJQuery"]);
        public static string TimesFormat { get; set; } = "hh:mm tt";

        #endregion

        #region HR
        public static string TotalLeaves => Convert.ToString(WebConfigurationManager.AppSettings["TotalLeaves"]);
        #endregion 
    }
}
