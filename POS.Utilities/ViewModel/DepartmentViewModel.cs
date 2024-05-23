using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }


        public static implicit operator DepartmentViewModel(Department dpt)
        {
            if (dpt == null)
                return null;

            return new DepartmentViewModel()
            {
                Id = dpt.Id,
                Name = dpt.Name,
                IsActive = dpt.IsActive,
            };
        }

        public static implicit operator Department(DepartmentViewModel dpt)
        {
            if (dpt == null)
                return null;

            return new Department()
            {
                Id = dpt.Id,
                Name = dpt.Name,
                IsActive = dpt.IsActive,
            };
        }
    }
}
