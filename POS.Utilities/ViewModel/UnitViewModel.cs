using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class UnitViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }


        public static implicit operator UnitViewModel(Unit unit)
        {
            if (unit == null)
                return null;

            return new UnitViewModel()
            {
                Id = unit.Id,
                Name = unit.Name,
                IsActive = unit.IsActive,
                CreationDate = unit.CreationDate,
                ModifyDate = unit.ModifyDate,
            };
        }

        public static implicit operator Unit(UnitViewModel unit)
        {
            if (unit == null)
                return null;

            return new Unit()
            {
                Id = unit.Id,
                Name = unit.Name,
                IsActive = unit.IsActive,
                CreationDate = unit.CreationDate,
                ModifyDate = unit.ModifyDate,
            };
        }


    }
}
