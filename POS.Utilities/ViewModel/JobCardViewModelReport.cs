using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public  class JobCardViewModelReport
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
    }
}
