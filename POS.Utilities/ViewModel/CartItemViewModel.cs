using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class CartItemViewModel
    {
        public CartItemViewModel()
        {
            ItemName = null;
            Quantity = null;
            //Price = null;
            //TaxAmount = null;
            //TaxPercentage = null;
            TableId = null;
            OrderId = null;
            Remarks = null;
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? OrderId { get; set; }
        public double? Quantity { get; set; }
        public double Price { get; set; }
        public double TaxAmount { get; set; }
        public double TaxPercentage { get; set; }
        public int? TableId { get; set; }
        public bool IsVoid { get; set; }
        public bool IsComplimentary { get; set; }
        public string Remarks { get; set; }

        public int? DepartmentId { get; set; }

        public static implicit operator CartItemViewModel(Item item)
        {
            if (item == null)
                return null;

            return new CartItemViewModel()
            {
                ItemId = item.Id,
            };
        }

        public static implicit operator Item(CartItemViewModel item)
        {
            if (item == null)
                return null;

            return new Item()
            {
                Id = item.ItemId,
            };

        }


    }
}
