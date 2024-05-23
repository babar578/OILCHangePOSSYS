using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public Nullable<System.DateTime> TransationDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator PurchaseViewModel(Purchase purchase)
        {
            if (purchase == null)
                return null;

            return new PurchaseViewModel()
            {
                Id = purchase.Id,
                Payment = purchase.Payment,
                TransationDate = purchase.TransationDate,
                CreationDate = purchase.CreationDate,
                ModificationDate = purchase.ModificationDate,
                IsActive = purchase.IsActive,
            };
        }

        public static implicit operator Purchase(PurchaseViewModel purchase)
        {
            if (purchase == null)
                return null;

            return new Purchase()
            {
                Id = purchase.Id,
                Payment = purchase.Payment,
                TransationDate = purchase.TransationDate,
                CreationDate = purchase.CreationDate,
                ModificationDate = purchase.ModificationDate,
                IsActive = purchase.IsActive
            };
        }
    }
}

