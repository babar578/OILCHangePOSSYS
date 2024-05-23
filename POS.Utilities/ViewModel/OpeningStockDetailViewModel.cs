using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
     public class OpeningStockDetailViewModel
    {
        public int Id { get; set; }
        public string VoucherLineNo { get; set; }
        public int RefId { get; set; }
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public double IssueQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public double Rate { get; set; }
        public double TotalRate { get; set; }
        public string LineRemarks { get; set; }
        public Nullable<double> Gst { get; set; }
        public Nullable<double> AdjustmentIN { get; set; }
        public Nullable<double> AdjustmentOUT { get; set; }
        public virtual OpeningStockHeadViewModel OpeningStockHead { get; set; }
        public string ItemName { get; set; }


        public static implicit operator OpeningStockDetailViewModel(OpeningDetail detail)
        {
            if (detail == null)
                return null;

            return new OpeningStockDetailViewModel()
            {
                Id = detail.Id,
                AdjustmentIN = detail.AdjustmentIN,
                AdjustmentOUT = detail.AdjustmentOUT,
                Gst = detail.Gst,
                ItemId = detail.ItemId,
                LineRemarks = detail.LineRemarks,
                Quantity = detail.Quantity,
                Rate = detail.Rate,
                TotalRate = detail.TotalRate,
                VoucherLineNo = detail.VoucherLineNo,
                ItemName = detail.ItemName,
            };
        }

        public static implicit operator OpeningDetail(OpeningStockDetailViewModel detail)
        {
            if (detail == null)
                return null;

            return new OpeningDetail()
            {
                Id = detail.Id,
                AdjustmentIN = detail.AdjustmentIN,
                AdjustmentOUT = detail.AdjustmentOUT,
                Gst = detail.Gst,
                ItemId = detail.ItemId,
                LineRemarks = detail.LineRemarks,
                Quantity = detail.Quantity,
                Rate = detail.Rate,
                TotalRate = detail.TotalRate,
                VoucherLineNo = detail.VoucherLineNo,
                ItemName = detail.ItemName,
            };
        }
    }
}
