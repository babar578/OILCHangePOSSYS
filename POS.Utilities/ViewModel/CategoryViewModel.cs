using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsRawMaterial { get; set; }

        public static implicit operator CategoryViewModel(Category category)
        {
            if (category == null)
                return null;

            return new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                CreationDate = category.CreationDate,
                ModifyDate = category.ModifyDate,
                IsRawMaterial = category.IsRawMaterial,
            };
        }

        public static implicit operator Category(CategoryViewModel category)
        {
            if (category == null)
                return null;

            return new Category()
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                CreationDate = category.CreationDate,
                ModifyDate = category.ModifyDate,
                IsRawMaterial = category.IsRawMaterial,
            };
        }

    }
}
