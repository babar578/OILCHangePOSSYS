using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class FloorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }


        public static implicit operator FloorViewModel(Floor floor)
        {
            if (floor == null)
                return null;

            return new FloorViewModel()
            {
                Id = floor.Id,
                Name = floor.Name,
                IsActive = floor.IsActive,
                CreationDate = floor.CreationDate,
                ModifyDate = floor.ModifyDate,
            };
        }

        public static implicit operator Floor(FloorViewModel floor)
        {
            if (floor == null)
                return null;

            return new Floor()
            {
                Id = floor.Id,
                Name = floor.Name,
                IsActive = floor.IsActive,
                CreationDate = floor.CreationDate,
                ModifyDate = floor.ModifyDate,
            };
        }

    }
}
