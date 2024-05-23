using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public Nullable<System.DateTime> TransationDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator SaleViewModel(Sale sale)
        {
            if (sale == null)
                return null;

            return new SaleViewModel()
            {
                Id = sale.Id,
                Payment = sale.Payment,
                TransationDate = sale.TransationDate,
                CreationDate = sale.CreationDate,
                ModificationDate = sale.ModificationDate,
                IsActive = sale.IsActive,
            };
        }

        public static implicit operator Sale(SaleViewModel sale)
        {
            if (sale == null)
                return null;

            return new Sale()
            {
                Id = sale.Id,
                Payment = sale.Payment,
                TransationDate = sale.TransationDate,
                CreationDate = sale.CreationDate,
                ModificationDate = sale.ModificationDate,
                IsActive = sale.IsActive
            };
        }
    }
}

