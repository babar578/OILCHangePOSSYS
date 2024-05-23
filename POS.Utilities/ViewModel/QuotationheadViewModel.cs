using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
 public   class QuotationheadViewModel
    {
        public QuotationheadViewModel()
        {
            QuotationDetails = new List<QuotationDetailViewModel>();
            Vendors = new List<CustomerViewModel>();
            RawItems = new List<ItemViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
            //warehouses = new List<WarehouseViewModel>();
            //receivedFroms = new List<ReceivedFromViewModel>();


        }

        public int Id { get; set; }
        public int VendorId { get; set; }
        public Nullable<int> Pono { get; set; }
        public Nullable<int> JobID { get; set; }
        public Nullable<System.DateTime> QuotationDate { get; set; }
        public Nullable<System.DateTime> DateOFSupplyDate { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<System.DateTime> VoildUptoDate { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> SurCharge { get; set; }
        public Nullable<double> GstCharges { get; set; }
        public Nullable<double> GrossAmount { get; set; }
        public Nullable<double> TotalNetAmount { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> WarehousesID { get; set; }
        public Nullable<int> ReceivedID { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public Nullable<System.DateTime> UnloadedTime { get; set; }
        public string VehicleNo { get; set; }
        public string TransportCompany { get; set; }
        public Nullable<double> TransportionCost { get; set; }

   

        // list 
        public List<ItemViewModel> RawItems { get; set; }

        public int ItemId { get; set; }
        public List<QuotationDetailViewModel> QuotationDetails { get; set; }
        public VendorViewModel Vendor { get; set; }
        public List<CustomerViewModel> Vendors { get; set; }

        public List<ItemStockViewModel> ItemStocks { get; set; }

        //public List<WarehouseViewModel> warehouses { get; set; }


        //public List<ReceivedFromViewModel> receivedFroms { get; set; }

        public string CreatedByName
        {
            get
            {
                if (CreatedBy > 0)
                {
                    return UserServices.GetUserNameById(Convert.ToInt32(CreatedBy));
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


        public static implicit operator QuotationheadViewModel(QuotationHead head)
        {
            if (head == null)
                return null;

            return new QuotationheadViewModel()
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
                ArrivalTime = head.ArrivalTime,
                UnloadedTime = head.UnloadedTime,
                VehicleNo = head.VehicleNo,
                TransportCompany = head.TransportCompany,
                TransportionCost = head.TransportionCost,
                ReceivedID = head.ReceivedID,
                WarehousesID = head.WarehousesID,
                JobID=head.JobID,
                QuotationDate=head.QuotationDate,
                DateOFSupplyDate=head.DateOFSupplyDate,
                PaymentDate=head.PaymentDate,
                VoildUptoDate=head.VoildUptoDate,
            };
        }

        public static implicit operator QuotationHead(QuotationheadViewModel head)
        {
            if (head == null)
                return null;

            return new QuotationHead()
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
                ArrivalTime = head.ArrivalTime,
                UnloadedTime = head.UnloadedTime,
                VehicleNo = head.VehicleNo,
                TransportCompany = head.TransportCompany,
                TransportionCost = head.TransportionCost,
                ReceivedID = head.ReceivedID,
                WarehousesID = head.WarehousesID,
                JobID = head.JobID,
                QuotationDate = head.QuotationDate,
                DateOFSupplyDate = head.DateOFSupplyDate,
                PaymentDate = head.PaymentDate,
                VoildUptoDate = head.VoildUptoDate,
            };
        }



    }
}
