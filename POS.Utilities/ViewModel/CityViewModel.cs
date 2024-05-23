using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class CityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }


        public static implicit operator CityViewModel(City city)
        {
            if (city == null)
                return null;

            return new CityViewModel()
            {
                Id = city.Id,
                Name = city.Name,
                IsActive = city.IsActive,
            };
        }

        public static implicit operator City(CityViewModel city)
        {
            if (city == null)
                return null;

            return new City()
            {
                Id = city.Id,
                Name = city.Name,
                IsActive = city.IsActive,
            };
        }
    }
}
