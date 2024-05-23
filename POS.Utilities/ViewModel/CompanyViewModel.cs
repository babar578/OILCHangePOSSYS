using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class CompanyViewModel
    {

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameUrdu { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }
        public string NTN { get; set; }
        public string LOGO { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CNIC { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsActive { get; set; }


        public static implicit operator CompanyViewModel(Company  city)
        {
            if (city == null)
                return null;

            return new CompanyViewModel()
            {
                Id = city.Id,
                CompanyName = city.CompanyName,
                CompanyNameUrdu = city.CompanyNameUrdu,
                PhoneNumber2 = city.PhoneNumber2,
                PhoneNumber3 = city.PhoneNumber3,
                NTN = city.NTN,

                LOGO = city.LOGO,

                Email = city.Email,

                Address = city.Address,
                CNIC = city.CNIC,
                CreationDate = city.CreationDate,
                ModifyDate = city.ModifyDate,

                IsActive = city.IsActive,

  
            };
        }

        public static implicit operator Company(CompanyViewModel city)
        {
            if (city == null)
                return null;

            return new Company()
            {
                Id = city.Id,
                CompanyName = city.CompanyName,
                CompanyNameUrdu = city.CompanyNameUrdu,
                PhoneNumber2 = city.PhoneNumber2,
                PhoneNumber3 = city.PhoneNumber3,
                NTN = city.NTN,

                LOGO = city.LOGO,

                Email = city.Email,

                Address = city.Address,
                CNIC = city.CNIC,
                CreationDate = city.CreationDate,
                ModifyDate = city.ModifyDate,

                IsActive = city.IsActive,

            };
        }
    }
}
