using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ItemViewModel
    {
        public ItemViewModel()
        {
            Categories = new List<CategoryViewModel>();
            SubCategories = new List<SubCategoryViewModel>();
            Units = new List<UnitViewModel>();
            Departments = new List<DepartmentViewModel>();
            ItemGroups = new List<ItemGroupViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public double Price { get; set; }
        public double PPrice { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        public Nullable<System.DateTime> PromotionStartDate { get; set; }
        public Nullable<System.DateTime> PromotionEndDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public int DepartmentId { get; set; }
        public bool IsRawMaterial { get; set; }
        public Nullable<int> ItemGroupId { get; set; }
        public string Remarks { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<SubCategoryViewModel> SubCategories { get; set; }
        public List<UnitViewModel> Units { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public List<ItemGroupViewModel> ItemGroups { get; set; }
        public Double BalanceQuantity { get; set; }
        public double Quantity { get; set; }
        public double SubTotal
        {
            get
            {
                if (Quantity > 0 && PPrice > 0)
                {
                    return (Quantity * PPrice);
                }
                else
                {
                    return 0;
                }
            }
        }

        public double QSubTotal
        {
            get
            {
                if (Quantity > 0 && Price > 0)
                {
                    return (Quantity * Price);
                }
                else
                {
                    return 0;
                }
            }
        }

        public string CategoryName
        {
            get
            {
                if (CategoryId > 0)
                {
                    return ItemServices.GetCategoryName(CategoryId ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }

        public string ItemGroupName
        {
            get
            {
                if (ItemGroupId > 0)
                {
                    return ItemServices.GetItemGroupName(ItemGroupId ?? 0);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public string SubCategoryName
        {
            get
            {
                if (SubCategoryId > 0)
                {
                    return ItemServices.GetSubCategoryName(SubCategoryId ?? 0);
                }
                else
                {
                    return " - ";
                }

            }
        }

        public string UnitName
        {
            get
            {
                if (UnitId > 0)
                {
                    return ItemServices.GetUnitName(UnitId ?? 0);
                }
                else
                {
                    return " - ";
                }

            }
        }

        public string DepartmentName
        {
            get
            {
                if (DepartmentId > 0)
                {
                    return ItemServices.GetDepartmentName(DepartmentId);
                }
                else
                {
                    return " - ";
                }

            }
        }

        public static implicit operator ItemViewModel(Item item)
        {
            if (item == null)
                return null;

            return new ItemViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                ModifyDate = item.ModifyDate,
                CreationDate = item.CreationDate,
                IsActive = item.IsActive,
                CategoryId = item.CategoryId,
                Discount = item.Discount,
                Price = item.Price,
                PromotionEndDate = item.PromotionEndDate,
                PromotionStartDate = item.PromotionStartDate,
                SubCategoryId = item.SubCategoryId,
                Tax = item.Tax,
                UnitId = item.UnitId,
                DepartmentId = item.DepartmentId,
                IsRawMaterial = item.IsRawMaterial,
                ItemGroupId = item.ItemGroupId,
                Remarks = item.Remarks,
            };
        }

        public static implicit operator Item(ItemViewModel item)
        {
            if (item == null)
                return null;

            return new Item()
            {
                Id = item.Id,
                Name = item.Name,
                ModifyDate = item.ModifyDate,
                CreationDate = item.CreationDate,
                IsActive = item.IsActive,
                CategoryId = item.CategoryId,
                Discount = item.Discount,
                Price = item.Price,
                PromotionEndDate = item.PromotionEndDate,
                PromotionStartDate = item.PromotionStartDate,
                SubCategoryId = item.SubCategoryId,
                Tax = item.Tax,
                UnitId = item.UnitId,
                DepartmentId = item.DepartmentId,
                IsRawMaterial = item.IsRawMaterial,
                ItemGroupId = item.ItemGroupId,
                Remarks = item.Remarks,
            };
        }

    }
}
