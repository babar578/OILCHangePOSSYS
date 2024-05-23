
//using POS.Database.DatabaseModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace POS.Utilities.Utilities
//{
//    public static class Seeder
//    {
//        public static void Seed()
//        {
//            using (POSEntities context = new POSEntities())
//            {
//                if (!context.Categories.Any())
//                {
//                    var categories = new List<Category>()
//                    {
//                      new Category{ Name="Salads", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Soups", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Chicken Entrees", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Beaf Entrees", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Seafood Entrees", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Pasta", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Pizza", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Burgers and Sandwiches", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Side Order", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Desserts", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                      new Category{ Name="Shakes & Smoothies", IsActive=true, CreationDate = DateTime.Now, ModifyDate= DateTime.Now },
//                    };
//                    context.Categories.AddRange(categories);
//                    context.SaveChanges();
//                }

//                if (!context.SubCategories.Any())
//                {
//                    var subCategories = new List<SubCategory>()
//                    {
//                      new SubCategory{ Name="AAA-01", CategoryId=1, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="AAA-02", CategoryId=1, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="AAA-03", CategoryId=1, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="BBB-01", CategoryId=2, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="BBB-02", CategoryId=2, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="BBB-03", CategoryId=2, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="CCC-01", CategoryId=3, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="CCC-02", CategoryId=3, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="CCC-03", CategoryId=3, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="DDD-01", CategoryId=4, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="DDD-02", CategoryId=4, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                      new SubCategory{ Name="DDD-03", CategoryId=4, IsActive= true, CreationDate= DateTime.Now, ModifyDate = DateTime.Now },
//                    };
//                    context.SubCategories.AddRange(subCategories);
//                    context.SaveChanges();
//                }

//                if (!context.UserTypes.Any())
//                {
//                    var types = new List<UserType>()
//                    {
//                      new UserType{ Name="Admin", IsActive = true},
//                      new UserType{ Name="Manager", IsActive = true},
//                      new UserType{ Name="User", IsActive = true},
//                    };
//                    context.UserTypes.AddRange(types);
//                    context.SaveChanges();
//                }

//                if (!context.Users.Any())
//                {
//                    var users = new List<User>()
//                    {
//                      new User{ UserName="admin", Password=Utility.Encrypt("test") , Email="admin@domain.com", CreationDate=DateTime.Now, ModifyDate=DateTime.Now, IsActive=true,  UserTypeId=1},
//                      new User{ UserName="manager", Password=Utility.Encrypt("test") , Email="admin@domain.com", CreationDate=DateTime.Now, ModifyDate=DateTime.Now, IsActive=true, UserTypeId=2},
//                      new User{ UserName="user", Password=Utility.Encrypt("test") , Email="user@domain.com", CreationDate=DateTime.Now, ModifyDate=DateTime.Now, IsActive=true, UserTypeId=3},
//                    };

//                    context.Users.AddRange(users);
//                    context.SaveChanges();
//                }

//                if (!context.Departments.Any())
//                {
//                    var departments = new List<Department>()
//                    {
//                      new Department{ Name="Kitchen",IsActive=true},
//                      new Department{ Name="Pizza & Desserts",IsActive=true},
//                      new Department{ Name="BAR",IsActive=true},
//                    };
//                    context.Departments.AddRange(departments);
//                    context.SaveChanges();
//                }

//                //if (!context.MainMenus.Any())
//                //{
//                //    var mainMenus = new List<MainMenu>()
//                //    {
//                //        new MainMenu{ Name="Dashboard", URL="/Home/Index", Icon="fas fa-tachometer-alt", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now },
//                //        new MainMenu{ Name="Item Setup", URL="/Home/Index", Icon="fas fa-file-alt", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now },
//                //        new MainMenu{ Name="Tables", URL="javascript: void(0)", Icon="fas fa-table", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now  },
//                //        new MainMenu{ Name="POS Panel", URL="/Home/POS", Icon="fas fa-desktop", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now  },
//                //        new MainMenu{ Name="Orders", URL="javascript: void(0)", Icon="fa-shopping-cart", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now  },
//                //        new MainMenu{ Name="Reports", URL="javascript: void(0)", Icon="fa-pencil-ruler", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now  },
//                //        new MainMenu{ Name="Users", URL="javascript: void(0)", Icon="fas fa-users", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now  },
//                //    };
//                //    context.MainMenus.AddRange(mainMenus);
//                //    context.SaveChanges();
//                //}


//                //if (!context.MainMenus.Where(p => p.ParentId != null).Any())
//                //{
//                //    var mainMenus = new List<MainMenu>()
//                //    {

//                //        new MainMenu{ Name="All Items", URL="/Menu/Items", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=15  },
//                //        new MainMenu{ Name="Promotion Items", URL="javascript: void(0)", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=15  },
//                //        new MainMenu{ Name="Categories", URL="/Menu/Categories", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=15  },
//                //        new MainMenu{ Name="Sub-Categories", URL="/Menu/SubCategories", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=15  },
//                //        new MainMenu{ Name="Unit", URL="/Menu/Units", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=15  },

//                //        new MainMenu{ Name="All Tables", URL="/Menu/FloorTables", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=16  },
//                //        new MainMenu{ Name="All Floors", URL="/Menu/Floors", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=16  },

//                //        new MainMenu{ Name="Closed Orders", URL="/Home/AllOrders", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=18  },
//                //        new MainMenu{ Name="Open Orders", URL="/Home/OpenOrders", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=18  },
//                //        new MainMenu{ Name="Open Orders", URL="/Home/DraftOrders", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=18  },

//                //        new MainMenu{ Name="All Users", URL="javascript: void(0)", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=20  },
//                //        new MainMenu{ Name="User Rights", URL="javascript: void(0)", Icon="far fa-circle", IsActive=true, CreationDate=DateTime.Now, ModifyDate=DateTime.Now, ParentId=20  },



//                //    };
//                //    context.MainMenus.AddRange(mainMenus);
//                //    context.SaveChanges();
//                //}


//                if (!context.UserRights.Any())
//                {
//                    var rights = new List<UserRight>()
//                    {
//                         new UserRight{ UserId=1, MenuId=14, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=15, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=16, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=17, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=18, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=19, ParentId=null, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=20, ParentId=null, IsActive = true},

//                         new UserRight{ UserId=1, MenuId=21, ParentId=15, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=22, ParentId=15, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=23, ParentId=15, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=24, ParentId=15, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=25, ParentId=15, IsActive = true},

//                         new UserRight{ UserId=1, MenuId=26, ParentId=16, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=27, ParentId=16, IsActive = true},

//                         new UserRight{ UserId=1, MenuId=28, ParentId=18, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=29, ParentId=18, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=30, ParentId=18, IsActive = true},

//                         new UserRight{ UserId=1, MenuId=31, ParentId=20, IsActive = true},
//                         new UserRight{ UserId=1, MenuId=32, ParentId=20, IsActive = true},
//                    };
//                    context.UserRights.AddRange(rights);
//                    context.SaveChanges();
//                }


//                if (!context.PaymentTypes.Any())
//                {
//                    var paymentTypes = new List<PaymentType>()
//                    {
//                        new PaymentType { Name="Cash", TaxPercentage=16, IsActive=true, CreationDate= DateTime.Now, ModifyDate= DateTime.Now},
//                        new PaymentType { Name="Keenu", TaxPercentage=16, IsActive=true, CreationDate= DateTime.Now, ModifyDate= DateTime.Now},
//                        new PaymentType { Name="Bank Alfalah", TaxPercentage=5, IsActive=true, CreationDate= DateTime.Now, ModifyDate= DateTime.Now},
//                        new PaymentType { Name="Partially", TaxPercentage=16, IsActive=true, CreationDate= DateTime.Now, ModifyDate= DateTime.Now},
//                    };
//                    context.PaymentTypes.AddRange(paymentTypes);
//                    context.SaveChanges();
//                }
//            }
//        }
//    }
//}
