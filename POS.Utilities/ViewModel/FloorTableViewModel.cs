using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class FloorTableViewModel
    {

        public FloorTableViewModel()
        {
            Floors = new List<FloorViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int FloorId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }

        public bool IsHold { get; set; }

        public FloorViewModel Floor { get; set; }
        public List<FloorViewModel> Floors { get; set; }

        public string FloorName
        {
            get
            {
                if (FloorId > 0)
                {
                    return ItemServices.GetFloorName(FloorId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator FloorTableViewModel(FloorTable table)
        {
            if (table == null)
                return null;

            return new FloorTableViewModel()
            {
                Id = table.Id,
                Name = table.Name,
                FloorId = table.FloorId,
                IsActive = table.IsActive,
                ModifyDate = table.ModifyDate,
                CreationDate = table.CreationDate,
                IsHold = table.IsHold,

            };
        }

        public static implicit operator FloorTable(FloorTableViewModel table)
        {
            if (table == null)
                return null;

            return new FloorTable()
            {
                Id = table.Id,
                Name = table.Name,
                FloorId = table.FloorId,
                IsActive = table.IsActive,
                ModifyDate = table.ModifyDate,
                CreationDate = table.CreationDate,
                IsHold = table.IsHold,
            };
        }


    }
}
