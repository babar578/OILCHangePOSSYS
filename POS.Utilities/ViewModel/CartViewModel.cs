using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Utilities.ViewModel
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Items = new List<ItemViewModel>();
            CartItems = new List<CartItemViewModel>();
        }
        //public OrderViewModel Order { get; set; }
        public ItemViewModel Item { get; set; }
        public ICollection<ItemViewModel> Items { get; set; }
        public ICollection<CartItemViewModel> CartItems { get; set; }

    }
}