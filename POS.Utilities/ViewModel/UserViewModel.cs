using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            UserTypes = new List<UserTypeViewModel>();
            Designations = new List<DesignationViewModel>();
            Customers = new List<CustomerViewModel>();
            getHistoryCars = new List<GetHistoryCarVoucherViewModel>();
            getHistoryCarss = new GetHistoryCarVoucherViewModel();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string Mobile { get; set; }
        public int UserTypeId { get; set; }
        public Nullable<int> DesignationId { get; set; }

        public List<UserTypeViewModel> UserTypes { get; set; }
        public List<DesignationViewModel> Designations { get; set; }
        public List<CustomerViewModel> Customers { get; set; }
        public List<GetHistoryCarVoucherViewModel> getHistoryCars { get; set; }
        public GetHistoryCarVoucherViewModel getHistoryCarss { get; set; }

        public System.DateTime DateDate { get; set; }

        public int CustomerID { get; set; }
        public static implicit operator UserViewModel(User user)
        {
            if (user == null)
                return null;

            return new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                CreationDate = user.CreationDate,
                IsActive = user.IsActive,
                ModifyDate = user.ModifyDate,
                Password = user.Password,
                UserName = user.UserName,
                CNIC = user.CNIC,
                DesignationId = user.DesignationId,
                FatherName = user.FatherName,
                FullName = user.FullName,
                Mobile = user.Mobile,
                UserTypeId = user.UserTypeId,
            };
        }

        public static implicit operator User(UserViewModel user)
        {
            if (user == null)
                return null;

            return new User()
            {
                Id = user.Id,
                Email = user.Email,
                CreationDate = user.CreationDate,
                IsActive = user.IsActive,
                ModifyDate = user.ModifyDate,
                Password = user.Password,
                UserName = user.UserName,
                UserTypeId = user.UserTypeId,
                Mobile = user.Mobile,
                FullName = user.FullName,
                FatherName = user.FatherName,
                DesignationId = user.DesignationId,
                CNIC = user.CNIC,
            };
        }

    }
}
