using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class WastageHeadViewModel
    {
        public WastageHeadViewModel()
        {
            WastageDetails = new List<WastageDetailViewModel>();
            ItemStocks = new List<ItemStockViewModel>();
        }
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }

        public DepartmentViewModel Department { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public LocationViewModel Location { get; set; }
        public List<LocationViewModel> Locations { get; set; }

        public int ItemId { get; set; }
        public List<WastageDetailViewModel> WastageDetails { get; set; }

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

        public static implicit operator WastageHeadViewModel(WastageHead head)
        {
            if (head == null)
                return null;

            return new WastageHeadViewModel()
            {
                Id = head.Id,
                DepartmentId = head.DepartmentId,
                LocationId = head.LocationId,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                DocNo = head.DocNo,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                TransactionDate = head.TransactionDate,
            };
        }

        public static implicit operator WastageHead(WastageHeadViewModel head)
        {
            if (head == null)
                return null;

            return new WastageHead()
            {
                Id = head.Id,
                DepartmentId = head.DepartmentId,
                LocationId = head.LocationId,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                DocNo = head.DocNo,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                TransactionDate = head.TransactionDate,
            };
        }

    }
}
