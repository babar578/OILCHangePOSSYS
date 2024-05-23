using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VendorPaymentTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator VendorPaymentTypeViewModel(VendorPaymentType type)
        {
            if (type == null)
                return null;

            return new VendorPaymentTypeViewModel()
            {
                Id = type.Id,
                Name = type.Name,
                IsActive = type.IsActive,
            };
        }

        public static implicit operator VendorPaymentType(VendorPaymentTypeViewModel type)
        {
            if (type == null)
                return null;

            return new VendorPaymentType()
            {
                Id = type.Id,
                Name = type.Name,
                IsActive = type.IsActive,
            };
        }


    }
}
