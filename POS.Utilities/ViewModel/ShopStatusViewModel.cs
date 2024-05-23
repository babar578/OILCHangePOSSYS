using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ShopStatusViewModel
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> DateOpen { get; set; }
        public Nullable<bool> OpenStatus { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
        public Nullable<bool> ClosedStatus { get; set; }
        public Nullable<int> ShopOpenUserId { get; set; }
        public Nullable<int> ShopCloseUserId { get; set; }


        public static implicit operator ShopStatusViewModel(ShopStatus status)
        {
            if (status == null)
                return null;

            return new ShopStatusViewModel()
            {
                Id = status.Id,
                ClosedStatus = status.ClosedStatus,
                DateClosed = status.DateClosed,
                DateOpen = status.DateOpen,
                OpenStatus = status.OpenStatus,
                ShopCloseUserId = status.ShopCloseUserId,
                ShopOpenUserId = status.ShopOpenUserId,
            };
        }

        public static implicit operator ShopStatus(ShopStatusViewModel status)
        {
            if (status == null)
                return null;

            return new ShopStatus()
            {
                Id = status.Id,
                ClosedStatus = status.ClosedStatus,
                DateClosed = status.DateClosed,
                DateOpen = status.DateOpen,
                OpenStatus = status.OpenStatus,
                ShopCloseUserId = status.ShopCloseUserId,
                ShopOpenUserId = status.ShopOpenUserId,
            };
        }
    }
}
