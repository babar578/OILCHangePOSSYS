using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
public    class VehicaljobCardViewModel
    {

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
        //public Nullable<int> NextService_Miles { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public string CustomerName { get; set; }


        public static implicit operator VehicaljobCardViewModel(VehicleJobCard item)
        {
            if (item == null)
                return null;

            return new VehicaljobCardViewModel()
            {
                Id = item.Id,
                JobNo = item.JobNo,
                JobData = item.JobData,
                CarRegistrationNo = item.CarRegistrationNo,
                ModelYear = item.ModelYear,
                Milleage = item.Milleage,
                DataTimeIn = item.DataTimeIn,
                CarMake = item.CarMake,
                CarModel = item.CarModel,
                CustomerContactNo = item.CustomerContactNo,

                EngineNo = item.EngineNo,
                VINno = item.VINno,
                Patrol = item.Patrol,
                Reporteddefect = item.Reporteddefect,
                CompleteActione = item.CompleteActione,
                VehicleBodyReportComents = item.VehicleBodyReportComents,
                ODoMeter = item.ODoMeter,


                Hours = item.Hours,
                //NextService_Miles = item.NextService_Miles,
                CreationDate = item.CreationDate,
                ModifyDate = item.ModifyDate,
                IsActive = item.IsActive,
                CustomerName = item.CustomerName,
            };
        }

        public static implicit operator VehicleJobCard(VehicaljobCardViewModel item)
        {
            if (item == null)
                return null;

            return new VehicleJobCard()
            {
                Id = item.Id,
                JobNo = item.JobNo,
                JobData = item.JobData,
                CarRegistrationNo = item.CarRegistrationNo,
                ModelYear = item.ModelYear,
                Milleage = item.Milleage,
                DataTimeIn = item.DataTimeIn,
                CarMake = item.CarMake,
                CarModel = item.CarModel,
                CustomerContactNo = item.CustomerContactNo,

                EngineNo = item.EngineNo,
                VINno = item.VINno,
                Patrol = item.Patrol,
                Reporteddefect = item.Reporteddefect,
                CompleteActione = item.CompleteActione,
                VehicleBodyReportComents = item.VehicleBodyReportComents,
                ODoMeter = item.ODoMeter,


                Hours = item.Hours,
                //NextService_Miles = item.NextService_Miles,
                CreationDate = item.CreationDate,
                ModifyDate = item.ModifyDate,
                IsActive = item.IsActive,
                CustomerName = item.CustomerName,
            };
        }



    }
}
