using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class MainMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }



        public static implicit operator MainMenuViewModel(MainMenu menu)
        {
            if (menu == null)
                return null;

            return new MainMenuViewModel()
            {
                Id = menu.Id,
                URL = menu.URL,
                CreationDate = menu.CreationDate,
                Icon = menu.Icon,
                IsActive = menu.IsActive,
                ModifyDate = menu.ModifyDate,
                Name = menu.Name,
                Priority = menu.Priority,
                ParentId = menu.ParentId,
            };
        }

        public static implicit operator MainMenu(MainMenuViewModel menu)
        {
            if (menu == null)
                return null;

            return new MainMenu()
            {
                Id = menu.Id,
                URL = menu.URL,
                CreationDate = menu.CreationDate,
                Icon = menu.Icon,
                IsActive = menu.IsActive,
                ModifyDate = menu.ModifyDate,
                Name = menu.Name,
                Priority = menu.Priority,
                ParentId = menu.ParentId,
            };
        }
    }
}
