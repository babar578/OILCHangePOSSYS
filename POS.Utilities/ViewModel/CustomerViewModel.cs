using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
  public  class CustomerViewModel
    {
        public int CustomerID { get; set; }
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CarNumber { get; set; }
        public string Reading { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<int> CityId { get; set; }
        public string Mobile { get; set; }
        public string Telephone1 { get; set; }
        public string CNIC { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsActive { get; set; }

        public CityViewModel City { get; set; }
        public List<CityViewModel> Cities { get; set; }
        
        public List<CustomerViewModel> Customerss { get; set; }
        public string CityName
        {
            get
            {
                if (CityId > 0)
                {
                    return VendorServices.GetCityName(CityId ??0 );
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator CustomerViewModel(Customer customer)
        {
            if (customer == null)
                return null;

            return new CustomerViewModel()
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CarNumber =customer.CarNumber,
                Reading = customer.Reading,
                Email = customer.Email,
                Address = customer.Address,
                CityId = customer.CityId,
                Mobile = customer.Mobile,
                Telephone1 = customer.Telephone1,
                CNIC = customer.CNIC,
                CreationDate = customer.CreationDate,
                ModifyDate = customer.ModifyDate,
                IsActive = customer.IsActive,
            }; 
        }


        public static implicit operator Customer (CustomerViewModel customer)
        {
            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                CarNumber = customer.CarNumber,
                Reading = customer.Reading,
                Email = customer.Email,
                Address = customer.Address,
                CityId = customer.CityId,
                Mobile = customer.Mobile,
                Telephone1 = customer.Telephone1,
                CNIC = customer.CNIC,
                CreationDate = customer.CreationDate,
                ModifyDate = customer.ModifyDate,
                IsActive = customer.IsActive,
            };
        }
    }
}
