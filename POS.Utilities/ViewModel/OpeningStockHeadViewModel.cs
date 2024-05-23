using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OpeningStockHeadViewModel
    {
        public OpeningStockHeadViewModel()
        {
            OpeningStockDetails = new List<OpeningStockDetailViewModel>();
            Vendors = new List<VendorViewModel>();
            RawItems = new List<ItemViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
        }
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> SurCharge { get; set; }
        public Nullable<double> GstCharges { get; set; }
        public Nullable<double> GrossAmount { get; set; }
        public Nullable<double> TotalNetAmount { get; set; }

        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public System.DateTime TransactionDate { get; set; }




        public List<OpeningStockDetailViewModel> OpeningStockDetails { get; set; }
        public VendorViewModel Vendor { get; set; }
        public List<VendorViewModel> Vendors { get; set; }

        public int ItemId { get; set; }
        public List<ItemViewModel> RawItems { get; set; }
        public String ItemName { get; set; }
        public List<ItemStockViewModel> ItemStocks { get; set; }
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


        public static implicit operator OpeningStockHeadViewModel(OpeningHead head)
        {
            if (head == null)
                return null;

            return new OpeningStockHeadViewModel()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                Discount = head.Discount,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                SurCharge = head.SurCharge,
                //VendorId = head.VendorId,
                GrossAmount = head.GrossAmount,
                GstCharges = head.GstCharges,
                TotalNetAmount = head.TotalNetAmount,
                TransactionDate = head.TransactionDate,
            };
        }

        public static implicit operator OpeningHead(OpeningStockHeadViewModel model)
        {
            if (model == null)
                return null;

            return new OpeningHead()
            {
                Id = model.Id,
                CreatedBy = model.CreatedBy,
                CreationDate = model.CreationDate,
                Discount = model.Discount,
                DocNo = model.DocNo,
                ModifyDate = model.ModifyDate,
                Remarks = model.Remarks,
                SurCharge = model.SurCharge,
                //VendorId = head.VendorId,
                GrossAmount = model.GrossAmount,
                GstCharges = model.GstCharges,
                TotalNetAmount = model.TotalNetAmount,
                TransactionDate = model.TransactionDate,
            };
        }
    }
}
