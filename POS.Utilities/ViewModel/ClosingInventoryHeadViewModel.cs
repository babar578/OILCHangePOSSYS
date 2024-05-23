using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ClosingInventoryHeadViewModel
    {
        public ClosingInventoryHeadViewModel()
        {
            ClosingInventoryDetails = new List<ClosingInventoryDetailViewModel>();
        }
        public int Id { get; set; }
        public string DocNo { get; set; }
        public string Remarks { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public int DeptmentId { get; set; }
        public List<ClosingInventoryDetailViewModel> ClosingInventoryDetails { get; set; }


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

        public static implicit operator ClosingInventoryHeadViewModel(ClosingInventoryHead head)
        {
            if (head == null)
                return null;

            return new ClosingInventoryHeadViewModel()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                TransactionDate = head.TransactionDate,
            };
        }

        public static implicit operator ClosingInventoryHead(ClosingInventoryHeadViewModel head)
        {
            if (head == null)
                return null;

            return new ClosingInventoryHead()
            {
                Id = head.Id,
                CreatedBy = head.CreatedBy,
                CreationDate = head.CreationDate,
                DocNo = head.DocNo,
                ModifyDate = head.ModifyDate,
                Remarks = head.Remarks,
                TransactionDate = head.TransactionDate,
            };
        }

    }
}
