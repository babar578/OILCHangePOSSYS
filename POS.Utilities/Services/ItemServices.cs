using POS.Database.DatabaseModel;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.Services
{
    public static class ItemServices
    {
        public static ItemViewModel GetItemcurrentById(int id)
        {
            ItemViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Items.Id, items.Id AS ItemId, isnull(itm.BalanceQuantity, 0) AS BalanceQuantity, Items.Name, Items.CategoryId, Items.UnitId, Items.Price, Items.Tax,Items.Discount, Items.PromotionStartDate, Items.PromotionEndDate, Items.IsActive, Items.CreationDate,Items.ModifyDate, Items.DepartmentId, Items.IsRawMaterial, Items.ItemGroupId, Items.Remarks from Items left outer join[dbo].[fn_ItemsStockInHand] ('02-02-1990', GETDATE())  itm on Items.Id = itm.ItemId  Where Items.Id={ id}";
                    returnValue = context.Database.SqlQuery<ItemViewModel>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static ItemViewModel GetItemcurrentbarcodeById(string  id)
        {
            ItemViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Items.Id, items.Id AS ItemId, isnull(itm.BalanceQuantity, 0) AS BalanceQuantity, Items.Name, Items.CategoryId, Items.UnitId, Items.Price, Items.Tax,Items.Discount, Items.PromotionStartDate, Items.PromotionEndDate, Items.IsActive, Items.CreationDate,Items.ModifyDate, Items.DepartmentId, Items.IsRawMaterial, Items.ItemGroupId, Items.Remarks from Items left outer join[dbo].[fn_ItemsStockInHand] ('02-02-1990', GETDATE())  itm on Items.Id = itm.ItemId  Where Items.Barcode='{id}'";
                    returnValue = context.Database.SqlQuery<ItemViewModel>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #region Item Functions
        public static bool AddItem(ItemViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Item entity = (Item)model;
                    context.Items.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static ItemViewModel GetItemById(int id)
        {
            ItemViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Items where Id={id}";
                    returnValue = context.Database.SqlQuery<Item>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetItemNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Items where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemViewModel> GetAllItems(bool IsRawMaterial)
        {
            List<ItemViewModel> returnValue = new List<ItemViewModel>();
            List<Item> items = new List<Item>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    if (IsRawMaterial)
                    {
                        string SQL = $"select * from Items where IsRawMaterial = 1";
                        items = context.Database.SqlQuery<Item>(SQL).ToList();
                    }
                    else
                    {
                        string SQL = $"select * from Items where IsRawMaterial = 1";
                        items = context.Database.SqlQuery<Item>(SQL).ToList();
                    }
                    returnValue = items.Select(p => (ItemViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemViewModel> GetAllItemsByCategoryWise(int categoryId)
        {
            List<ItemViewModel> returnValue = new List<ItemViewModel>();
            List<Item> items = new List<Item>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Items where CategoryId={categoryId}";
                    items = context.Database.SqlQuery<Item>(SQL).ToList();
                    returnValue = items.Select(p => (ItemViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemViewModel> GetItemsByName()
        {
            List<ItemViewModel> returnValue = new List<ItemViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Items";
                    var Items = context.Database.SqlQuery<Item>(SQL).ToList();
                    returnValue = Items.Select(p => (ItemViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateItem(ItemViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Items.Where(p => p.Id == model.Id).SingleOrDefault();  //GetItemById(model.Id);
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Name))
                            find.Name = model.Name;
                        if (model.CategoryId > 0)
                            find.CategoryId = model.CategoryId;
                        if (model.SubCategoryId > 0)
                            find.SubCategoryId = model.SubCategoryId;
                        if (model.UnitId > 0)
                            find.UnitId = model.UnitId;
                        if (model.DepartmentId > 0)
                            find.DepartmentId = model.DepartmentId;
                        if (model.ItemGroupId > 0)
                            find.ItemGroupId = model.ItemGroupId;
                        if (model.Price > 0)
                            find.Price = model.Price;
                        if (model.Tax > 0)
                            find.Tax = model.Tax;
                        if (model.Discount > 0)
                            find.Discount = model.Discount;
                        if (model.PromotionStartDate != DateTime.MinValue)
                            find.PromotionStartDate = model.PromotionStartDate;
                        if (model.PromotionEndDate != DateTime.MinValue)
                            find.PromotionEndDate = model.PromotionEndDate;
                        if (!string.IsNullOrWhiteSpace(model.Remarks))
                            find.Remarks = model.Remarks;
                        find.ModifyDate = DateTime.Now;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteItem(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Items.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Items.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableItem(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Items.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableItem(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Items.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region Category Functions
        public static bool AddCategory(CategoryViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Category entity = (Category)model;
                    context.Categories.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static CategoryViewModel GetCategoryById(int id)
        {
            CategoryViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Categories where Id={id}";
                    returnValue = context.Database.SqlQuery<Category>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetCategoryName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Categories where id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<CategoryViewModel> GetAllCategories(bool IsRawMaterial)
        {
            List<CategoryViewModel> returnValue = new List<CategoryViewModel>();
            List<Category> categories = new List<Category>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    if (IsRawMaterial)
                    {
                        string SQL = $"Select * from Categories where IsRawMaterial = 1";
                        categories = context.Database.SqlQuery<Category>(SQL).ToList();
                    }
                    else
                    {
                        string SQL = $"Select * from Categories where IsRawMaterial = 0";
                        categories = context.Database.SqlQuery<Category>(SQL).ToList();
                    }
                    returnValue = categories.Select(p => (CategoryViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<CategoryViewModel> GetCategoriesByName()
        {
            List<CategoryViewModel> returnValue = new List<CategoryViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Id , Name from Categories";
                    var Categories = context.Database.SqlQuery<Category>(SQL).ToList();
                    returnValue = Categories.Select(p => (CategoryViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateCategory(CategoryViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetCategoryById(model.Id);

                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Categories set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Categories.Where(p => p.Id == id).SingleOrDefault(); //GetCategoryById(id);
                    if (del != null)
                    {
                        context.Categories.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetCategoryById(id);
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetCategoryById(id);
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region SubCategory Functions
        public static bool AddSubCategory(SubCategoryViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    SubCategory entity = (SubCategory)model;
                    context.SubCategories.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static SubCategoryViewModel GetSubCategoryById(int id)
        {
            SubCategoryViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from SubCategories where Id={id}";
                    returnValue = context.Database.SqlQuery<SubCategory>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetSubCategoryName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from SubCategories where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<SubCategoryViewModel> GetAllSubCategories(string search = null)
        {
            List<SubCategoryViewModel> returnValue = new List<SubCategoryViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from SubCategories";
                    var SubCategories = context.Database.SqlQuery<SubCategory>(SQL).ToList();
                    returnValue = SubCategories.Select(p => (SubCategoryViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<SubCategoryViewModel> GetSubCategoriesByName()
        {
            List<SubCategoryViewModel> returnValue = new List<SubCategoryViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Id, Name from SubCategories";
                    var SubCategories = context.Database.SqlQuery<SubCategory>(SQL).ToList();
                    returnValue = SubCategories.Select(p => (SubCategoryViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<SubCategoryViewModel> GetSubCategoryByCategory(int category)
        {
            List<SubCategoryViewModel> returnValue = new List<SubCategoryViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from SubCategories where CategoryId={category}";
                    var SubCategories = context.Database.SqlQuery<SubCategory>(SQL).ToList();
                    returnValue = SubCategories.Select(p => (SubCategoryViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateSubCategory(SubCategoryViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetSubCategoryById(model.Id);

                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@CategoryId",model.CategoryId),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"Update SubCategories Set Name=@Name, CategoryId=@CategoryId, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteSubCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.SubCategories.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.SubCategories.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableSubCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.SubCategories.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableSubCategory(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.SubCategories.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
        #region Unit Functions
        public static bool AddUnit(UnitViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Unit entity = (Unit)model;
                    context.Units.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static UnitViewModel GetUnitById(int id)
        {
            UnitViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Units where Id={id}";
                    returnValue = context.Database.SqlQuery<Unit>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetUnitName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Units where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<UnitViewModel> GetAllUnits(string search = null)
        {
            List<UnitViewModel> returnValue = new List<UnitViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Units";
                    var Unit = context.Database.SqlQuery<Unit>(SQL).ToList();
                    returnValue = Unit.Select(p => (UnitViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<UnitViewModel> GetUnitsByName()
        {
            List<UnitViewModel> returnValue = new List<UnitViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Units";
                    var Unit = context.Database.SqlQuery<Unit>(SQL).ToList();
                    returnValue = Unit.Select(p => (UnitViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateUnit(UnitViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetUnitById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Units set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteUnit(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = GetUnitById(id);
                    if (del != null)
                    {
                        context.Units.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableUnit(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetUnitById(id);
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableUnit(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetUnitById(id);
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region Floor Functions
        public static bool AddFloor(FloorViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Floor entity = (Floor)model;
                    context.Floors.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static FloorViewModel GetFloorById(int id)
        {
            FloorViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Floors where Id={id}";
                    returnValue = context.Database.SqlQuery<Floor>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetFloorName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Floors where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<FloorViewModel> GetAllFloors(string search = null)
        {
            List<FloorViewModel> returnValue = new List<FloorViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Floors";
                    var Floor = context.Database.SqlQuery<Floor>(SQL).ToList();
                    returnValue = Floor.Select(p => (FloorViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<FloorViewModel> GetFloorsByName()
        {
            List<FloorViewModel> returnValue = new List<FloorViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Floors";
                    var Floor = context.Database.SqlQuery<Floor>(SQL).ToList();
                    returnValue = Floor.Select(p => (FloorViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateFloor(FloorViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetFloorById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Floors set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteFloor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Floors.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Floors.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableFloor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Floors.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableFloor(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Floors.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region FloorTable Functions
        public static bool AddFloorTable(FloorTableViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    FloorTable entity = (FloorTable)model;
                    context.FloorTables.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static FloorTableViewModel GetFloorTableById(int id)
        {
            FloorTableViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from FloorTables where Id={id}";
                    returnValue = context.Database.SqlQuery<FloorTable>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetFloorTableName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from FloorTables where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<FloorTableViewModel> GetAllFloorTables(string search = null)
        {
            List<FloorTableViewModel> returnValue = new List<FloorTableViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from FloorTables";
                    var FloorTable = context.Database.SqlQuery<FloorTable>(SQL).ToList();
                    returnValue = FloorTable.Select(p => (FloorTableViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<FloorTableViewModel> GetAllFloorTablesByFloorId(int floorId)
        {
            List<FloorTableViewModel> returnValue = new List<FloorTableViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from FloorTables Where FloorId = {floorId}";
                    var FloorTable = context.Database.SqlQuery<FloorTable>(SQL).ToList();
                    returnValue = FloorTable.Select(p => (FloorTableViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<FloorTableViewModel> GetFloorTablesByName()
        {
            List<FloorTableViewModel> returnValue = new List<FloorTableViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from FloorTables";
                    var FloorTable = context.Database.SqlQuery<FloorTable>(SQL).ToList();
                    returnValue = FloorTable.Select(p => (FloorTableViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateFloorTable(FloorTableViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetFloorTableById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@FloorId",model.FloorId),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update FloorTables set Name=@Name, FloorId=@FloorId, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool IsHoldTable(int tableId)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetFloorTableById(tableId);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",tableId),
                             new SqlParameter("@IsHold",true),
                        };
                        string SQL = $"update FloorTables set IsHold=@IsHold where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool IsReleaseTable(int tableId)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetFloorTableById(tableId);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",tableId),
                             new SqlParameter("@IsHold",false),
                        };
                        string SQL = $"update FloorTables set IsHold=@IsHold where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteFloorTable(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.FloorTables.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.FloorTables.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableFloorTable(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.FloorTables.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableFloorTable(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.FloorTables.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
        #region CartItems
        public static CartItemViewModel GetCartItemById(int id)
        {
            CartItemViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    returnValue = context.Items.Where(p => p.Id == id).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
        #region PaymentType Functions
        public static bool AddPaymentType(PaymentTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    PaymentType entity = (PaymentType)model;
                    context.PaymentTypes.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static PaymentTypeViewModel GetPaymentTypeById(int id)
        {
            PaymentTypeViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from PaymentTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<PaymentType>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetPaymentTypeName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Name from PaymentTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<PaymentTypeViewModel> GetAllPaymentTypes(string search = null)
        {
            List<PaymentTypeViewModel> returnValue = new List<PaymentTypeViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from PaymentTypes";
                    var PaymentType = context.Database.SqlQuery<PaymentType>(SQL).ToList();
                    returnValue = PaymentType.Select(p => (PaymentTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<PaymentTypeViewModel> GetPaymentTypesByName()
        {
            List<PaymentTypeViewModel> returnValue = new List<PaymentTypeViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from PaymentTypes";
                    var PaymentType = context.Database.SqlQuery<PaymentType>(SQL).ToList();
                    returnValue = PaymentType.Select(p => (PaymentTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdatePaymentType(PaymentTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetPaymentTypeById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update PaymentTypes set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeletePaymentType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.PaymentTypes.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.PaymentTypes.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisablePaymentType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.PaymentTypes.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnablePaymentType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.PaymentTypes.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion
        #region Department Functions
        public static bool AddDepartment(DepartmentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Department entity = (Department)model;
                    context.Departments.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static DepartmentViewModel GetDepartmentById(int id)
        {
            DepartmentViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Departments where Id={id}";
                    returnValue = context.Database.SqlQuery<Department>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetDepartmentName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Departments where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<DepartmentViewModel> GetAllDepartments(string search = null)
        {
            List<DepartmentViewModel> returnValue = new List<DepartmentViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Departments";
                    var Department = context.Database.SqlQuery<Department>(SQL).ToList();
                    returnValue = Department.Select(p => (DepartmentViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<DepartmentViewModel> GetDepartmentsByName()
        {
            List<DepartmentViewModel> returnValue = new List<DepartmentViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Departments";
                    var Department = context.Database.SqlQuery<Department>(SQL).ToList();
                    returnValue = Department.Select(p => (DepartmentViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateDepartment(DepartmentViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetDepartmentById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Departments set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteDepartment(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Departments.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Departments.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableDepartment(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Departments.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableDepartment(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Departments.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        #endregion
        #region ItemGroup Functions
        public static bool AddItemGroup(ItemGroupViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    ItemGroup entity = (ItemGroup)model;
                    context.ItemGroups.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static ItemGroupViewModel GetItemGroupById(int id)
        {
            ItemGroupViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ItemGroups where Id={id}";
                    returnValue = context.Database.SqlQuery<ItemGroup>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetItemGroupName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from ItemGroups where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemGroupViewModel> GetAllItemGroups(string search = null)
        {
            List<ItemGroupViewModel> returnValue = new List<ItemGroupViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ItemGroups";
                    var ItemGroup = context.Database.SqlQuery<ItemGroup>(SQL).ToList();
                    returnValue = ItemGroup.Select(p => (ItemGroupViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<ItemGroupViewModel> GetItemGroupsByName()
        {
            List<ItemGroupViewModel> returnValue = new List<ItemGroupViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from ItemGroups";
                    var ItemGroup = context.Database.SqlQuery<ItemGroup>(SQL).ToList();
                    returnValue = ItemGroup.Select(p => (ItemGroupViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateItemGroup(ItemGroupViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetItemGroupById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                        };
                        string SQL = $"update ItemGroups set Name=@Name where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteItemGroup(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.ItemGroups.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.ItemGroups.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DisableItemGroup(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.ItemGroups.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = false;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool EnableItemGroup(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.ItemGroups.Where(p => p.Id == id).SingleOrDefault();
                    if (find != null)
                    {
                        find.IsActive = true;
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        #endregion



        #region Job Card Functions
        public static ResponseDto AddJobCard(VehicaljobCardViewModel model)
        {
            ResponseDto responseDto = new ResponseDto();
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    VehicleJobCard entity = (VehicleJobCard)model;
                    context.VehicleJobCards.Add(entity);
                    context.SaveChanges();
                    responseDto.PrintId = entity.Id;
                    responseDto.Message = "Job Card Add Success";
                    responseDto.Status = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return responseDto;
        }
        public static VehicaljobCardViewModel GetVehicalJobCardById(int id)
        {
            VehicaljobCardViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VehicleJobCard where Id={id}";
                    returnValue = context.Database.SqlQuery<VehicleJobCard>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetVehicalJobName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from VehicleJobCard where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static List<VehicaljobCardViewModel> GetAllJobCards(string search = null)
        {
            List<VehicaljobCardViewModel> returnValue = new List<VehicaljobCardViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from VehicleJobCard";
                    var Unit = context.Database.SqlQuery<VehicleJobCard>(SQL).ToList();
                    returnValue = Unit.Select(p => (VehicaljobCardViewModel)p).ToList();
                }
            }
            catch (Exception ex )
            {
                var s = ex.ToString();
            }
            return returnValue;
        }
        public static List<VehicaljobCardViewModel> GetVehicalJobsByName()
        {
            List<VehicaljobCardViewModel> returnValue = new List<VehicaljobCardViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from VehicleJobCard";
                    var Unit = context.Database.SqlQuery<VehicleJobCard>(SQL).ToList();
                    returnValue = Unit.Select(p => (VehicaljobCardViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool UpdateJobCard(VehicaljobCardViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetVehicalJobCardById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.CarMake),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Units set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
                        context.Database.ExecuteSqlCommand(SQL, parameters);
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static bool DeleteJobCard(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = GetVehicalJobCardById(id);
                    if (del != null)
                    {
                        context.VehicleJobCards.Remove(del);
                        context.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        //public static bool DisableJobCard(int id)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            var find = GetUnitById(id);
        //            if (find != null)
        //            {
        //                find.IsActive = false;
        //                context.SaveChanges();
        //                returnValue = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}
        //public static bool EnableUnit(int id)
        //{
        //    bool returnValue = false;
        //    try
        //    {
        //        using (POSEntities context = new POSEntities())
        //        {
        //            var find = GetUnitById(id);
        //            if (find != null)
        //            {
        //                find.IsActive = true;
        //                context.SaveChanges();
        //                returnValue = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return returnValue;
        //}
        #endregion

        #region Company Functions
        public static bool AddComapny(CompanyViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Company entity = (Company)model;
                    context.Companies.Add(entity);
                    context.SaveChanges();
                    returnValue = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static  CompanyViewModel GetCompanyById()
        {
            CompanyViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Company ";
                    returnValue = context.Database.SqlQuery<Company>(SQL).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
            }
            return returnValue;
        }
    

     

    
      
        #endregion


    }
}
