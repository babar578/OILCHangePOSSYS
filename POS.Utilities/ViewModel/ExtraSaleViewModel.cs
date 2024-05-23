using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
     public class ExtraSaleViewModel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBY { get; set; }
        public string Reason { get; set; }
        public string username { get; set; }
        public static implicit operator ExtraSaleViewModel(ExtraSale area)
        {
            if (area == null)
                return null;

            return new ExtraSaleViewModel()
            {
                Id = area.Id,
                Amount = area.Amount,
                IsActive = area.IsActive,
                CreationDate = area.CreationDate,
                CreatedBY = area.CreatedBY,
                Reason = area.Reason,
            };
        }
        public static implicit operator ExtraSale(ExtraSaleViewModel area)
        {
            if (area == null)
                return null;

            return new ExtraSale()
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
