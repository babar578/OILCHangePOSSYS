using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public double TaxAmount { get; set; }
        public double Discount { get; set; }
        public Nullable<int> TableId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string Remarks { get; set; }
        public bool IsVoid { get; set; }
        public bool IsComplimentary { get; set; }

        public Nullable<int> DepartmentId { get; set; }
        public Nullable<double> AlignmentAmount { get; set; }
        public Nullable<double> wheelBalanceAmount { get; set; }

        public OrderViewModel Order { get; set; }



        public static implicit operator OrderItemViewModel(OrderItem orderItem)
        {
            if (orderItem == null)
                return null;

            return new OrderItemViewModel()
            {
                Id = orderItem.Id,
                ModifyDate = orderItem.ModifyDate,
                CreationDate = orderItem.CreationDate,
                Discount = orderItem.Discount,
                ItemName = orderItem.ItemName,
                OrderId = orderItem.OrderId,
                Price = orderItem.Price,
                TaxAmount = orderItem.TaxAmount,
                Quantity = orderItem.Quantity,
                TableId = orderItem.TableId,
                ItemId = orderItem.ItemId,
                Remarks = orderItem.Remarks,
                TotalPrice = orderItem.TotalPrice,
                IsComplimentary = orderItem.IsComplimentary,
                IsVoid = orderItem.IsVoid,
                DepartmentId = orderItem.DepartmentId,

                //AlignmentAmount = orderItem.AlignmentAmount,
                //wheelBalanceAmount = orderItem.wheelBalanceAmount,
            };
        }

        public static implicit operator OrderItem(OrderItemViewModel orderItem)
        {
            if (orderItem == null)
                return null;

            return new OrderItem()
            {
                Id = orderItem.Id,
                ModifyDate = orderItem.ModifyDate,
                CreationDate = orderItem.CreationDate,
                Discount = orderItem.Discount,
                ItemName = orderItem.ItemName,
                OrderId = orderItem.OrderId,
                Price = orderItem.Price,
                TaxAmount = orderItem.TaxAmount,
                Quantity = orderItem.Quantity,
                ItemId = orderItem.ItemId,
                TotalPrice = orderItem.TotalPrice,
                Remarks = orderItem.Remarks,
                TableId = orderItem.TableId,
                IsComplimentary = orderItem.IsComplimentary,
                IsVoid = orderItem.IsVoid,
                DepartmentId = orderItem.DepartmentId,

                //AlignmentAmount = orderItem.AlignmentAmount,
                //wheelBalanceAmount = orderItem.wheelBalanceAmount,
            };
        }

        public static implicit operator OrderItemViewModel(CartItemViewModel item)
        {
            if (item == null)
                return null;

            return new OrderItemViewModel()
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                OrderId = item.OrderId ?? 0,
                Quantity = (double)item.Quantity,
                Price = item.Price,
                TableId = item.TableId,
                TaxAmount = item.TaxAmount,
                IsVoid = item.IsVoid,
                IsComplimentary = item.IsComplimentary,
                Remarks = item.Remarks,
                DepartmentId = item.DepartmentId,
               
            };
        }

        public static implicit operator CartItemViewModel(OrderItemViewModel item)
        {
            if (item == null)
                return null;

            return new CartItemViewModel()
            {
                ItemId = item.ItemId ?? 0,
                ItemName = item.ItemName,
                OrderId = item.OrderId,
                Quantity = item.Quantity,
                Price = item.Price,
                TableId = item.TableId,
                TaxAmount = item.TaxAmount,
                IsVoid = item.IsVoid,
                IsComplimentary = item.IsComplimentary,
                Remarks = item.Remarks,
                DepartmentId = item.DepartmentId,

            };

        }
    }
}
