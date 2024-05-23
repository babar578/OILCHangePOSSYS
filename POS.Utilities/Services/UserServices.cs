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
    public static class UserServices
    {
        #region User Functions

        public static bool AddUser(UserViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    User entity = (User)model;
                    context.Users.Add(entity);
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

        public static UserViewModel GetUserById(int id)
        {
            UserViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Users where Id={id}";
                    returnValue = context.Database.SqlQuery<User>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        public static string GetUserNameById(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select UserName from Users where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserViewModel> GetAllUsers()
        {
            List<UserViewModel> returnValue = new List<UserViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Users";
                    var Users = context.Database.SqlQuery<User>(SQL).ToList();
                    returnValue = Users.Select(p => (UserViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserViewModel> GetUsers()
        {
            List<UserViewModel> returnValue = new List<UserViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"Select Id, UserName from Users";
                    var Users = context.Database.SqlQuery<User>(SQL).ToList();
                    returnValue = Users.Select(p => (UserViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateUser(UserViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Users.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.UserName))
                            find.UserName = model.UserName;
                        if (!string.IsNullOrWhiteSpace(model.Password))
                            find.Password = model.Password;
                        if (!string.IsNullOrWhiteSpace(model.Email))
                            find.Email = model.Email;
                        if (!string.IsNullOrWhiteSpace(model.FullName))
                            find.FullName = model.FullName;
                        if (!string.IsNullOrWhiteSpace(model.FatherName))
                            find.FatherName = model.FatherName;
                        if (!string.IsNullOrWhiteSpace(model.CNIC))
                            find.CNIC = model.CNIC;
                        if (!string.IsNullOrWhiteSpace(model.Mobile))
                            find.Mobile = model.Mobile;
                        if (model.UserTypeId > 0)
                            find.UserTypeId = model.UserTypeId;
                        if (model.DesignationId > 0)
                            find.DesignationId = model.DesignationId;

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

        public static bool ChangePassword(UserViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Users.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Password))
                            find.Password = model.Password;
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

        public static bool DeleteUser(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Users.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Users.Remove(del);
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

        public static bool DisableAccount(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Users.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableAccount(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Users.Where(p => p.Id == id).SingleOrDefault();
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

        public static UserViewModel UserLogin(string username, string password)
        {
            UserViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    returnValue = context.Users.Where(p => p.UserName == username && p.Password == password).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {

                var re = ex.ToString();
            }
            return returnValue;
        }


        #endregion

        #region UserType Functions

        public static bool AddUserType(UserTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    UserType entity = (UserType)model;
                    context.UserTypes.Add(entity);
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

        public static UserTypeViewModel GetUserTypeById(int id)
        {
            UserTypeViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<UserType>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetUserTypeName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from UserTypes where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserTypeViewModel> GetAllUserTypes(string search = null)
        {
            List<UserTypeViewModel> returnValue = new List<UserTypeViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserTypes";
                    var UserType = context.Database.SqlQuery<UserType>(SQL).ToList();
                    returnValue = UserType.Select(p => (UserTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserTypeViewModel> GetUserTypesByName()
        {
            List<UserTypeViewModel> returnValue = new List<UserTypeViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from UserTypes";
                    var UserType = context.Database.SqlQuery<UserType>(SQL).ToList();
                    returnValue = UserType.Select(p => (UserTypeViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateUserType(UserTypeViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetUserTypeById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update UserTypes set Name=@Name, ModifyDate=@ModifyDate where id = @ID";
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

        public static bool DeleteUserType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.UserTypes.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.UserTypes.Remove(del);
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

        public static bool DisableUserType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.UserTypes.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableUserType(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.UserTypes.Where(p => p.Id == id).SingleOrDefault();
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

        #region Designation Functions

        public static bool AddDesignation(DesignationViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    Designation entity = (Designation)model;
                    context.Designations.Add(entity);
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

        public static DesignationViewModel GetDesignationById(int id)
        {
            DesignationViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Designations where Id={id}";
                    returnValue = context.Database.SqlQuery<Designation>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetDesignationName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Designations where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<DesignationViewModel> GetAllDesignations()
        {
            List<DesignationViewModel> returnValue = new List<DesignationViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from Designations";
                    var Designation = context.Database.SqlQuery<Designation>(SQL).ToList();
                    returnValue = Designation.Select(p => (DesignationViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<DesignationViewModel> GetDesignationsByName()
        {
            List<DesignationViewModel> returnValue = new List<DesignationViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from Designations";
                    var Designation = context.Database.SqlQuery<Designation>(SQL).ToList();
                    returnValue = Designation.Select(p => (DesignationViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateDesignation(DesignationViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetDesignationById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@Name",model.Name),
                             //new SqlParameter("@ModifyDate",DateTime.Now),
                        };
                        string SQL = $"update Designations set Name=@Name where id = @ID";
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

        public static bool DeleteDesignation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.Designations.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.Designations.Remove(del);
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

        public static bool DisableDesignation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Designations.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableDesignation(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.Designations.Where(p => p.Id == id).SingleOrDefault();
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

        #region PrintInfo Functions

        public static bool AddPrintInfo(PrintInfoViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    PrintInfo entity = (PrintInfo)model;
                    context.PrintInfoes.Add(entity);
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

        public static PrintInfoViewModel GetPrintInfoById(int id)
        {
            PrintInfoViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from PrintInfo where Id={id}";
                    returnValue = context.Database.SqlQuery<PrintInfo>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static PrintInfoViewModel GetPrintInfoByDepartmentId(int id)
        {
            PrintInfoViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from PrintInfo where DepartmentId={id}";
                    returnValue = context.Database.SqlQuery<PrintInfo>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetPrintInfoName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from PrintInfo where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<PrintInfoViewModel> GetAllPrintInfoes(string search = null)
        {
            List<PrintInfoViewModel> returnValue = new List<PrintInfoViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from PrintInfoes";
                    var PrintInfo = context.Database.SqlQuery<PrintInfo>(SQL).ToList();
                    returnValue = PrintInfo.Select(p => (PrintInfoViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<PrintInfoViewModel> GetPrintInfoesByName()
        {
            List<PrintInfoViewModel> returnValue = new List<PrintInfoViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from PrintInfoes";
                    var PrintInfo = context.Database.SqlQuery<PrintInfo>(SQL).ToList();
                    returnValue = PrintInfo.Select(p => (PrintInfoViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdatePrintInfo(PrintInfoViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = GetPrintInfoById(model.Id);
                    if (find != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                             new SqlParameter("@ID",model.Id),
                             new SqlParameter("@PrinterName",model.PrinterName),
                             new SqlParameter("@IP_Address",model.IP_Address),
                             new SqlParameter("@Port",model.Port),
                             new SqlParameter("@DepartmentId",model.DepartmentId),
                        };
                        string SQL = $"update PrintInfoes set PrinterName=@PrinterName, IP_Address=@IP_Address, Port=@Port, DepartmentId=@DepartmentId where id = @ID";
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

        public static bool DeletePrintInfo(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.PrintInfoes.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.PrintInfoes.Remove(del);
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

        #region MainMenu Functions

        public static bool AddMainMenu(MainMenuViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    MainMenu entity = (MainMenu)model;
                    context.MainMenus.Add(entity);
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

        public static MainMenuViewModel GetMainMenuById(int id)
        {
            MainMenuViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from MainMenu where Id={id}";
                    returnValue = context.Database.SqlQuery<MainMenu>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static string GetMainMenuName(int id)
        {
            string returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from MainMenu where Id={id}";
                    returnValue = context.Database.SqlQuery<string>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<MainMenuViewModel> GetAllParentMenus()
        {
            List<MainMenuViewModel> returnValue = new List<MainMenuViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from MainMenu Where ParentId IS NULL ";
                    var MainMenu = context.Database.SqlQuery<MainMenu>(SQL).ToList();
                    returnValue = MainMenu.Select(p => (MainMenuViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }


        public static List<MainMenuViewModel> GetAllChildMenus(int ParentId)
        {
            List<MainMenuViewModel> returnValue = new List<MainMenuViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from MainMenu where parentId = {ParentId}";
                    var MainMenu = context.Database.SqlQuery<MainMenu>(SQL).ToList();
                    returnValue = MainMenu.Select(p => (MainMenuViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }



        public static List<MainMenuViewModel> GetAllMainMenusByParentId(int parentId)
        {
            List<MainMenuViewModel> returnValue = new List<MainMenuViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from MainMenu where ParentId={parentId}";
                    var MainMenu = context.Database.SqlQuery<MainMenu>(SQL).ToList();
                    returnValue = MainMenu.Select(p => (MainMenuViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<MainMenuViewModel> GetMainMenusByName()
        {
            List<MainMenuViewModel> returnValue = new List<MainMenuViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select Name from MainMenu";
                    var MainMenu = context.Database.SqlQuery<MainMenu>(SQL).ToList();
                    returnValue = MainMenu.Select(p => (MainMenuViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool UpdateMainMenu(MainMenuViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.MainMenus.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Name))
                            find.Name = model.Name;
                        if (!string.IsNullOrWhiteSpace(model.URL))
                            find.URL = model.URL;
                        if (!string.IsNullOrWhiteSpace(model.Icon))
                            find.Icon = model.Icon;
                        if (model.ParentId > 0)
                            find.ParentId = model.ParentId;
                        if (model.Priority > 0)
                            find.Priority = model.Priority;

                        model.ModifyDate = DateTime.Now;
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

        public static bool DeleteMainMenu(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.MainMenus.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.MainMenus.Remove(del);
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

        public static bool DisableMainMenu(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.MainMenus.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableMainMenu(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.MainMenus.Where(p => p.Id == id).SingleOrDefault();
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

        #region UserRight Functions

        public static bool AddUserRight(UserRightViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    UserRight entity = (UserRight)model;
                    context.UserRights.Add(entity);
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

        public static bool AddNewUserRight(int UserId, List<int> rights)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    if (rights.Count > 0)
                    {

                        var findPreviousRights = context.UserRights.Where(p => p.UserId == UserId).ToList();
                        if (findPreviousRights?.Count > 0)
                        {
                            context.UserRights.RemoveRange(findPreviousRights);
                        }
                        foreach (var rID in rights)
                        {
                            var find = context.MainMenus.Where(p => p.Id == rID).SingleOrDefault();

                            if (find != null)
                            {
                                UserRight entity = new UserRight
                                {
                                    UserId = UserId,
                                    MenuId = find.Id,
                                    ParentId = find.ParentId,
                                    IsActive = true,
                                };

                                context.UserRights.Add(entity);
                                context.SaveChanges();
                                returnValue = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static UserRightViewModel GetUserRightById(int id)
        {
            UserRightViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserRights where Id={id}";
                    returnValue = context.Database.SqlQuery<UserRight>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static UserRightViewModel GetUserRightByUserId(int userId)
        {
            UserRightViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserRights where UserId={userId}";
                    returnValue = context.Database.SqlQuery<UserRight>(SQL).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserRightViewModel> GetAllUserRights()
        {
            List<UserRightViewModel> returnValue = new List<UserRightViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserRights";
                    var UserRight = context.Database.SqlQuery<UserRight>(SQL).ToList();
                    returnValue = UserRight.Select(p => (UserRightViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserRightViewModel> GetAllUserRightsByUserId(int userId)
        {
            List<UserRightViewModel> returnValue = new List<UserRightViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserRights where UserId={userId} and IsActive='true'";
                    var UserRight = context.Database.SqlQuery<UserRight>(SQL).ToList();
                    returnValue = UserRight.Select(p => (UserRightViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static List<UserRightViewModel> GetAllUserRights_SubMenu_ByParentId(int parentId, int userId)
        {
            List<UserRightViewModel> returnValue = new List<UserRightViewModel>();
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from UserRights where ParentId={parentId} and UserId = {userId} and IsActive='true'";
                    var UserRight = context.Database.SqlQuery<UserRight>(SQL).ToList();
                    returnValue = UserRight.Select(p => (UserRightViewModel)p).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }



        public static bool UpdateUserRight(UserRightViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.UserRights.Where(p => p.Id == model.Id).SingleOrDefault();
                    if (find != null)
                    {

                        if (model.UserId > 0)
                            find.UserId = model.UserId;
                        if (model.ParentId > 0)
                            find.ParentId = model.ParentId;
                        if (model.MenuId > 0)
                            find.MenuId = model.MenuId;

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

        public static bool DeleteUserRight(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var del = context.UserRights.Where(p => p.Id == id).SingleOrDefault();
                    if (del != null)
                    {
                        context.UserRights.Remove(del);
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

        public static bool DisableUserRight(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.UserRights.Where(p => p.Id == id).SingleOrDefault();
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

        public static bool EnableUserRight(int id)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    var find = context.UserRights.Where(p => p.Id == id).SingleOrDefault();
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

        #region ShopStatus

        public static bool ShopOpenStatus(ShopStatusViewModel model)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    ShopStatus entity = (ShopStatus)model;
                    context.ShopStatuses.Add(entity);
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


        public static bool ShopCloseStatus(int shopCloseUserId)
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL_01 = $"SELECT TOP 1 * FROM ShopStatuses ORDER BY ID DESC";
                    var lastRecord = context.Database.SqlQuery<ShopStatus>(SQL_01).FirstOrDefault();

                    if (lastRecord != null)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ID",lastRecord.Id),
                            new SqlParameter("@OpenStatus",false),
                            new SqlParameter("@ClosedStatus",true),
                            new SqlParameter("@DateClosed",DateTime.Now),
                            new SqlParameter("@ShopCloseUserId",shopCloseUserId),
                        };
                        string SQL_02 = $"Update ShopStatuses set OpenStatus=@OpenStatus, ClosedStatus=@ClosedStatus, DateClosed=@DateClosed, ShopCloseUserId=@ShopCloseUserId  WHERE Id = @ID";
                        context.Database.ExecuteSqlCommand(SQL_02, parameters);
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

        public static ShopStatusViewModel GetShopStatusById(int id)
        {
            ShopStatusViewModel returnValue = null;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    string SQL = $"select * from ShopStatuses where Id={id}";
                    returnValue = context.Database.SqlQuery<ShopStatus>(SQL).SingleOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }

        public static bool CheckShopOpenStatus()
        {
            bool returnValue = false;
            try
            {
                using (POSEntities context = new POSEntities())
                {
                    // string SQL = $"SELECT TOP 1 OpenStatus FROM ShopStatuses WHERE CONVERT(DATETIME, CONVERT(VARCHAR, DateOpen, 101)) = '{DateTime.Today}' ORDER BY ID DESC";
                    string SQL = $"SELECT TOP 1 OpenStatus FROM ShopStatuses ";
                    var check = context.Database.SqlQuery<bool>(SQL).SingleOrDefault();
                    if (check)
                    {
                        returnValue = check;
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


    }
}
