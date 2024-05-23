using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
     public class ExpenseViewModel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBY { get; set; }
        public string Reason { get; set; }
        public string username { get; set; }
        public static implicit operator ExpenseViewModel(Expense area)
        {
            if (area == null)
                return null;

            return new ExpenseViewModel()
            {
                Id = area.Id,
                Amount = area.Amount,
                IsActive = area.IsActive,
                CreationDate = area.CreationDate,
                CreatedBY = area.CreatedBY,
                Reason = area.Reason,
            };
        }
        public static implicit operator Expense(ExpenseViewModel area)
        {
            if (area == null)
                return null;

            return new Expense()
            {
                Id = area.Id,
                Amount = area.Amount,
                IsActive = area.IsActive,
                CreationDate = area.CreationDate,
                CreatedBY = area.CreatedBY,
                Reason = area.Reason,
            };
        }
    }
}
