using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator LocationViewModel(Location location)
        {
            if (location == null)
                return null;

            return new LocationViewModel()
            {
                Id = location.Id,
                Name = location.Name,
                IsActive = location.IsActive,
            };
        }

        public static implicit operator Location(LocationViewModel location)
        {
            if (location == null)
                return null;

            return new Location()
            {
                Id = location.Id,
                Name = location.Name,
                IsActive = location.IsActive,
            };
        }

    }
}
