using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ReturnToVendorHeadViewModel
    {
        public ReturnToVendorHeadViewModel()
        {
            ReturnToVendorDetails = new List<ReturnToVendorDetailViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
            Vendors = new List<VendorViewModel>();
        }
        public int Id { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }

        public double Discount { get; set; }
        public double SurCharge { get; set; }
        public double GstCharges { get; set; }
        public double GrossAmount { get; set; }
        public double TotalNetAmount { get; set; }

        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }

        public List<VendorViewModel> Vendors { get; set; }

        public int ItemId { get; set; }
        public List<ItemStockViewModel> ItemStocks { get; set; }
        public List<ReturnToVendorDetailViewModel> ReturnToVendorDetails { get; set; }


        public string CreatedByName
        {
            get
            {
                if (CreatedBy > 0)
                {
                    return UserServices.GetUserNameById(CreatedBy);
                }
                else
                {
                    return " - ";
                }
            }
        }

        public string VendorName
        {
            get
            {
                if (VendorId > 0)
                {
                    return VendorServices.GetVendorNameById(VendorId ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator ReturnToVendorHeadViewModel(ReturnToVendorHead head)
        {
            if (head == null)
                return null;

            return new ReturnToVendorHeadViewModel()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                VendorId = head.VendorId,
                DepartmentId = head.DepartmentId,
                LocationId = head.LocationId,
                TransactionDate = head.TransactionDate,
                Discount = head.Discount,
                GrossAmount = head.GrossAmount,
                GstCharges = head.GstCharges,
                SurCharge = head.SurCharge,
                TotalNetAmount = head.TotalNetAmount,
            };
        }

        public static implicit operator ReturnToVendorHead(ReturnToVendorHeadViewModel head)
        {
            if (head == null)
                return null;

            return new ReturnToVendorHead()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                VendorId = head.VendorId,
                DepartmentId = head.DepartmentId,
                LocationId = head.LocationId,
                TransactionDate = head.TransactionDate,
                Discount = head.Discount,
                GrossAmount = head.GrossAmount,
                GstCharges = head.GstCharges,
                SurCharge = head.SurCharge,
                TotalNetAmount = head.TotalNetAmount,
            };
        }
    }
}
