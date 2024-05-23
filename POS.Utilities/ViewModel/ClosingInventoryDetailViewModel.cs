using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ClosingInventoryDetailViewModel
    {
        public int Id { get; set; }
        public string VoucherLineNo { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalRate { get; set; }
        public string LineRemarks { get; set; }
        public ClosingInventoryHeadViewModel ClosingInventoryDetail { get; set; }


        public static implicit operator ClosingInventoryDetailViewModel(ClosingInventoryDetail detail)
        {
            if (detail == null)
                return null;

            return new ClosingInventoryDetailViewModel()
            {
                Id = detail.Id,
                VoucherLineNo = detail.VoucherLineNo,
                ItemId = detail.ItemId,
                ItemName = detail.ItemName,
                LineRemarks = detail.LineRemarks,
                Quantity = detail.Quantity,
                Rate = detail.Rate,
                TotalRate = detail.TotalRate,
            };
        }

        public static implicit operator ClosingInventoryDetail(ClosingInventoryDetailViewModel detail)
        {
            if (detail == null)
                return null;

            return new ClosingInventoryDetail()
            {
                Id = detail.Id,
                VoucherLineNo = detail.VoucherLineNo,
                ItemId = detail.ItemId,
                ItemName = detail.ItemName,
                LineRemarks = detail.LineRemarks,
                Quantity = detail.Quantity,
                Rate = detail.Rate,
                TotalRate = detail.TotalRate,
            };
        }

    }
}
