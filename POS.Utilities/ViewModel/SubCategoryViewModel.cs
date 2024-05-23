using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class SubCategoryViewModel
    {
        public SubCategoryViewModel()
        {
            Categories = new List<CategoryViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public List<CategoryViewModel> Categories { get; set; }

        public string CategoryName
        {
            get
            {
                if (CategoryId > 0)
                {
                    return ItemServices.GetCategoryName(CategoryId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator SubCategoryViewModel(SubCategory subCategory)
        {
            if (subCategory == null)
                return null;

            return new SubCategoryViewModel()
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                IsActive = subCategory.IsActive,
                ModifyDate = subCategory.ModifyDate,
                CreationDate = subCategory.CreationDate,

            };
        }

        public static implicit operator SubCategory(SubCategoryViewModel subCategory)
        {
            if (subCategory == null)
                return null;

            return new SubCategory()
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                CategoryId = subCategory.CategoryId,
                IsActive = subCategory.IsActive,
                ModifyDate = subCategory.ModifyDate,
                CreationDate = subCategory.CreationDate,
            };
        }

    }
}
