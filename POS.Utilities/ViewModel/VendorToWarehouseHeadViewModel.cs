using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VendorToWarehouseHeadViewModel
    {
        public VendorToWarehouseHeadViewModel()
        {
            VendorToWarehouseDetails = new List<VendorToWarehouseDetailViewModel>();
            Vendors = new List<VendorViewModel>();
            RawItems = new List<ItemViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
        }
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public double Discount { get; set; }
        public double SurCharge { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public double GstCharges { get; set; }
        public double GrossAmount { get; set; }
        public double TotalNetAmount { get; set; }

        public List<VendorToWarehouseDetailViewModel> VendorToWarehouseDetails { get; set; }
        public VendorViewModel Vendor { get; set; }
        public List<VendorViewModel> Vendors { get; set; }

        public int ItemId { get; set; }
        public List<ItemViewModel> RawItems { get; set; }
        public ItemViewModel RawItemss { get; set; }
        public String ItemName { get; set; }
        public List<ItemStockViewModel> ItemStocks { get; set; }

        public double Quantity { get; set; }
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
                    return VendorServices.GetVendorNameById(VendorId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator VendorToWarehouseHeadViewModel(VendorToWarehouseHead head)
        {
            if (head == null)
                return null;

            return new VendorToWarehouseHeadViewModel()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                Discount = head.Discount,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                SurCharge = head.SurCharge,
                VendorId = head.VendorId,
                GrossAmount = head.GrossAmount,
                GstCharges = head.GstCharges,
                TotalNetAmount = head.TotalNetAmount,
                TransactionDate = head.TransactionDate,
            };
        }

        public static implicit operator VendorToWarehouseHead(VendorToWarehouseHeadViewModel head)
        {
            if (head == null)
                return null;

            return new VendorToWarehouseHead()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                Discount = head.Discount,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                SurCharge = head.SurCharge,
                VendorId = head.VendorId,
                GrossAmount = head.GrossAmount,
                GstCharges = head.GstCharges,
                TotalNetAmount = head.TotalNetAmount,
                TransactionDate = head.TransactionDate,
            };
        }
    }
}
