using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class UserRightViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int MenuId { get; set; }
        public bool IsActive { get; set; }

        public MainMenuViewModel MainMenu { get; set; }
        public UserViewModel User { get; set; }


        public string MenuName
        {
            get
            {
                if (MenuId > 0)
                {
                    return UserServices.GetMainMenuName(MenuId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator UserRightViewModel(UserRight right)
        {
            if (right == null)
                return null;

            return new UserRightViewModel()
            {
                Id = right.Id,
                MenuId = right.MenuId,
                ParentId = right.ParentId,
                IsActive = right.IsActive,
                UserId = right.UserId,
            };
        }

        public static implicit operator UserRight(UserRightViewModel right)
        {
            if (right == null)
                return null;

            return new UserRight()
            {
                Id = right.Id,
                MenuId = right.MenuId,
                ParentId = right.ParentId,
                IsActive = right.IsActive,
                UserId = right.UserId,
            };
        }

    }
}
