using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class UserTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator UserTypeViewModel(UserType type)
        {
            if (type == null)
                return null;

            return new UserTypeViewModel()
            {
                Id = type.Id,
                Name = type.Name,
                IsActive = type.IsActive,
            };
        }

        public static implicit operator UserType(UserTypeViewModel type)
        {
            if (type == null)
                return null;

            return new UserType()
            {
                Id = type.Id,
                Name = type.Name,
                IsActive = type.IsActive,
            };
        }
    }
}
