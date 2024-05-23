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
    public class JobCardHeadViewModel
    {
        public JobCardHeadViewModel()
        {
            JobCardDetails = new List<JobcardDetailViewModel>();
            Vendors = new List<VendorViewModel>();
            RawItems = new List<ItemViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
        }


        public int Id { get; set; }
        public string JobNo { get; set; }
        public Nullable<System.DateTime> JobData { get; set; }
        public string CarRegistrationNo { get; set; }
        public string ModelYear { get; set; }
        public string Milleage { get; set; }
        public Nullable<System.DateTime> DataTimeIn { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string CustomerContactNo { get; set; }
        public string EngineNo { get; set; }
        public string VINno { get; set; }
        public string Patrol { get; set; }
        public string Reporteddefect { get; set; }
        public string CompleteActione { get; set; }
        public string VehicleBodyReportComents { get; set; }
        public Nullable<int> ODoMeter { get; set; }
        public Nullable<int> Hours { get; set; }
        public Nullable<int> NextService_Miles { get; set; }
        public int VendorId { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public double Discount { get; set; }
        public double SurCharge { get; set; }
        public double GstCharges { get; set; }
        public double GrossAmount { get; set; }
        public double TotalNetAmount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public List<JobcardDetailViewModel> JobCardDetails { get; set; }
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


        public static implicit operator JobCardHeadViewModel(JobCardHead head)
        {
            if (head == null)
                return null;

            return new JobCardHeadViewModel()
            {
                Id = head.Id,
                JobNo = head.JobNo,
                JobData = head.JobData,
                CarRegistrationNo = head.CarRegistrationNo,
                ModelYear = head.ModelYear,
                Milleage = head.Milleage,
                DataTimeIn = head.DataTimeIn,
                CarMake = head.CarMake,
                CarModel = head.CarModel,
                CustomerContactNo = head.CustomerContactNo,

                EngineNo = head.EngineNo,
                VINno = head.VINno,
                Patrol = head.Patrol,
                Reporteddefect = head.Reporteddefect,
                CompleteActione = head.CompleteActione,
                VehicleBodyReportComents = head.VehicleBodyReportComents,
                ODoMeter = head.ODoMeter,


                Hours = head.Hours,
                NextService_Miles = head.NextService_Miles,
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

        public static implicit operator JobCardHead(JobCardHeadViewModel head)
        {
            if (head == null)
                return null;

            return new JobCardHead()
            {
                Id = head.Id,
                JobNo = head.JobNo,
                JobData = head.JobData,
                CarRegistrationNo = head.CarRegistrationNo,
                ModelYear = head.ModelYear,
                Milleage = head.Milleage,
                DataTimeIn = head.DataTimeIn,
                CarMake = head.CarMake,
                CarModel = head.CarModel,
                CustomerContactNo = head.CustomerContactNo,

                EngineNo = head.EngineNo,
                VINno = head.VINno,
                Patrol = head.Patrol,
                Reporteddefect = head.Reporteddefect,
                CompleteActione = head.CompleteActione,
                VehicleBodyReportComents = head.VehicleBodyReportComents,
                ODoMeter = head.ODoMeter,


                Hours = head.Hours,
                NextService_Miles = head.NextService_Miles,
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
