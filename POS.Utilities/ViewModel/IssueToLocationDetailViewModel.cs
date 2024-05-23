using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class IssueToLocationDetailViewModel
    {
        public int Id { get; set; }
        public string VoucherLineNo { get; set; }
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double TotalRate { get; set; }
        public string LineRemarks { get; set; }
        public Nullable<double> Gst { get; set; }
        public Nullable<double> AdjustmentIN { get; set; }
        public Nullable<double> AdjustmentOUT { get; set; }
        public IssueToLocationHeadViewModel IssueToLocationHead { get; set; }
        public string ItemName { get; set; }
        //custm
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public System.DateTime IssuanceDate { get; set; }

        public string Categoriesname { get; set; }
        public static implicit operator IssueToLocationDetailViewModel(IssueToLocationDetail detail)
        {
            if (detail == null)
                return null;

            return new IssueToLocationDetailViewModel()
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

        public static implicit operator IssueToLocationDetail(IssueToLocationDetailViewModel detail)
        {
            if (detail == null)
                return null;

            return new IssueToLocationDetail()
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
