using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ItemGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator ItemGroupViewModel(ItemGroup group)
        {
            if (group == null)
                return null;

            return new ItemGroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                IsActive = group.IsActive,
            };
        }

        public static implicit operator ItemGroup(ItemGroupViewModel group)
        {
            if (group == null)
                return null;

            return new ItemGroup()
            {
                Id = group.Id,
                Name = group.Name,
                IsActive = group.IsActive,
            };
        }
    }
}
