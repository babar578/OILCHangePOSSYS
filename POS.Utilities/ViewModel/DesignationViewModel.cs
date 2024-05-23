using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class DesignationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator DesignationViewModel(Designation designation)
        {
            if (designation == null)
                return null;

            return new DesignationViewModel()
            {
                Id = designation.Id,
                Name = designation.Name,
                IsActive = designation.IsActive,
            };
        }

        public static implicit operator Designation(DesignationViewModel designation)
        {
            if (designation == null)
                return null;

            return new Designation()
            {
                Id = designation.Id,
                Name = designation.Name,
                IsActive = designation.IsActive,
            };
        }
    }
}
