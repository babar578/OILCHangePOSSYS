using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class PaymentTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double TaxPercentage { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }


        public static implicit operator PaymentTypeViewModel(PaymentType payment)
        {
            if (payment == null)
                return null;

            return new PaymentTypeViewModel()
            {
                Id = payment.Id,
                Name = payment.Name,
                IsActive = payment.IsActive,
                CreationDate = payment.CreationDate,
                ModifyDate = payment.ModifyDate,
                TaxPercentage = payment.TaxPercentage,
            };
        }

        public static implicit operator PaymentType(PaymentTypeViewModel payment)
        {
            if (payment == null)
                return null;

            return new PaymentType()
            {
                Id = payment.Id,
                Name = payment.Name,
                IsActive = payment.IsActive,
                CreationDate = payment.CreationDate,
                ModifyDate = payment.ModifyDate,
                TaxPercentage = payment.TaxPercentage,
            };
        }
    }
}
