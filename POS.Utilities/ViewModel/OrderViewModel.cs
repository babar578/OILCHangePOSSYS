using AutoMapper;
using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
        }
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public double GSTPerentage { get; set; }
        public double FOCDiscount { get; set; }
        public double DiscountPercentage { get; set; }
        public double BankDiscount { get; set; }
        public double DiscountVoucher { get; set; }
        public double Tip { get; set; }
        public double ServiceChargesPerentage { get; set; }
        public double GstCharged { get; set; }
        public double DiscountCharged { get; set; }
        public double ServiceCharged { get; set; }
        public double GrossAmount { get; set; }
        public double ComplimantryAmount { get; set; }
        public double VoidAmount { get; set; }
        public double TotalNetAmount { get; set; }
        public double ReceiptTotalCash { get; set; }
        public double ReceiptTotalCredit { get; set; }
        public double Change { get; set; }
        public Nullable<int> TableId { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsUpdateMode { get; set; }
        public bool IsPayment { get; set; }
        public Nullable<System.DateTime> ShopOpenOrderDate { get; set; }
        public Nullable<int> OrderTypeId { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public Nullable<int> NoOfGuest { get; set; }
        public bool IsVoid { get; set; }
        public bool IsComplimentary { get; set; }
        public string Reason { get; set; }
        public bool returnValue { get; set; }
        public string massage { get; set; }
        public string CustomerName { get; set; }
        public string CarNo { get; set; }
        public string Reading { get; set; }
        public double LastReading { get; set; }
        public double CurrentReading { get; set; }
        public double AlignmentAmount { get; set; }
        public double wheelBalanceAmount { get; set; }
        public double NextReading { get; set; }
        public DateTime? NextReadingDate { get; set; }
        public string OilItemName { get; set; }
        public double NitrogenGas { get; set; }
        public double TPMS { get; set; }
        public string PaymentTypeName
        {
            get
            {
                if (PaymentTypeId > 0)
                {
                    return ItemServices.GetPaymentTypeName(PaymentTypeId ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }

        public string TableName
        {
            get
            {
                if (TableId > 0)
                {
                    return ItemServices.GetFloorTableName(TableId ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }

        public string UserName
        {
            get
            {
                if (CreateBy > 0)
                {
                    return UserServices.GetUserNameById(CreateBy ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator OrderViewModel(Order order)
        {
            if (order == null)
                return null;

            return new OrderViewModel()
            {
                Id = order.Id,
                DiscountPercentage = order.DiscountPercentage,
                CreationDate = order.CreationDate,
                PaymentTypeId = order.PaymentTypeId,
                BankDiscount = order.BankDiscount,
                FOCDiscount = order.FOCDiscount,
                GSTPerentage = order.GSTPerentage,
                InvoiceNumber = order.InvoiceNumber,
                ModifyDate = order.ModifyDate,
                ServiceChargesPerentage = order.ServiceChargesPerentage,
                Tip = order.Tip,
                TableId = order.TableId,
                DiscountVoucher = order.DiscountVoucher,
                Change = order.Change,
                GrossAmount = order.GrossAmount,
                ReceiptTotalCash = order.ReceiptTotalCash,
                ReceiptTotalCredit = order.ReceiptTotalCredit,
                TotalNetAmount = order.TotalNetAmount,
                DiscountCharged = order.DiscountCharged,
                GstCharged = order.GstCharged,
                ServiceCharged = order.ServiceCharged,
                IsUpdateMode = order.IsUpdateMode,
                IsPayment = order.IsPayment,
                CreateBy = order.CreateBy,
                UpdateBy = order.UpdateBy,
                ShopOpenOrderDate = order.ShopOpenOrderDate,
                OrderTypeId = order.OrderTypeId,
                NoOfGuest = order.NoOfGuest ?? 0,
                IsComplimentary = order.IsComplimentary,
                IsVoid = order.IsVoid,
                Reason = order.Reason,

                AlignmentAmount = order.AlignmentAmount ??0,
                wheelBalanceAmount = order.wheelBalanceAmount??0,

                NitrogenGas = order.NitrogenGas ?? 0,
                TPMS = order.TPMS ?? 0,
            };
        }

        public static implicit operator Order(OrderViewModel order)
        {
            if (order == null)
                return null;

            return new Order()
            {
                Id = order.Id,
                DiscountPercentage = order.DiscountPercentage,
                CreationDate = order.CreationDate,
                PaymentTypeId = order.PaymentTypeId,
                BankDiscount = order.BankDiscount,
                FOCDiscount = order.FOCDiscount,
                GSTPerentage = order.GSTPerentage,
                InvoiceNumber = order.InvoiceNumber,
                ModifyDate = order.ModifyDate,
                ServiceChargesPerentage = order.ServiceChargesPerentage,
                Tip = order.Tip,
                TableId = order.TableId,
                DiscountVoucher = order.DiscountVoucher,
                Change = order.Change,
                GrossAmount = order.GrossAmount,
                ReceiptTotalCash = order.ReceiptTotalCash,
                ReceiptTotalCredit = order.ReceiptTotalCredit,
                TotalNetAmount = order.TotalNetAmount,
                DiscountCharged = order.DiscountCharged,
                GstCharged = order.GstCharged,
                ServiceCharged = order.ServiceCharged,
                IsUpdateMode = order.IsUpdateMode,
                IsPayment = order.IsPayment,
                CreateBy = order.CreateBy,
                UpdateBy = order.UpdateBy,
                ShopOpenOrderDate = order.ShopOpenOrderDate,
                OrderTypeId = order.OrderTypeId,
                NoOfGuest = order.NoOfGuest ?? 0,
                IsVoid = order.IsVoid,
                IsComplimentary = order.IsComplimentary,
                Reason = order.Reason,
                AlignmentAmount = order.AlignmentAmount,
                wheelBalanceAmount = order.wheelBalanceAmount,
                NitrogenGas = order.NitrogenGas,
                TPMS = order.TPMS ,
            };
        }
    }
}
