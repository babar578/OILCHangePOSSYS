using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VendorViewModel
    {
        public VendorViewModel()
        {
            Cities = new List<CityViewModel>();
        }
        public int Id { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string Mobile { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string GstNo { get; set; }
        public string CNIC { get; set; }
        public string NTN { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public System.DateTime BusinessStartDate { get; set; }

        public bool IsActive { get; set; }
        public CityViewModel City { get; set; }
        public List<CityViewModel> Cities { get; set; }

        public string CityName
        {
            get
            {
                if (CityId > 0)
                {
                    return VendorServices.GetCityName(CityId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public static implicit operator VendorViewModel(Vendor vendor)
        {
            if (vendor == null)
                return null;

            return new VendorViewModel()
            {
                Id = vendor.Id,
                Address = vendor.Address,
                BusinessStartDate = vendor.BusinessStartDate,
                CityId = vendor.CityId,
                CNIC = vendor.CNIC,
                ContactPerson = vendor.ContactPerson,
                CreationDate = vendor.CreationDate,
                Email = vendor.Email,
                GstNo = vendor.GstNo,
                Mobile = vendor.Mobile,
                ModifyDate = vendor.ModifyDate,
                NTN = vendor.NTN,
                Telephone1 = vendor.Telephone1,
                Telephone2 = vendor.Telephone2,
                VendorName = vendor.VendorName,
                IsActive = vendor.IsActive,
            };
        }

        public static implicit operator Vendor(VendorViewModel vendor)
        {
            if (vendor == null)
                return null;

            return new Vendor()
            {
                Id = vendor.Id,
                Address = vendor.Address,
                BusinessStartDate = vendor.BusinessStartDate,
                CityId = vendor.CityId,
                CNIC = vendor.CNIC,
                ContactPerson = vendor.ContactPerson,
                CreationDate = vendor.CreationDate,
                Email = vendor.Email,
                GstNo = vendor.GstNo,
                Mobile = vendor.Mobile,
                ModifyDate = vendor.ModifyDate,
                NTN = vendor.NTN,
                Telephone1 = vendor.Telephone1,
                Telephone2 = vendor.Telephone2,
                VendorName = vendor.VendorName,
                IsActive = vendor.IsActive,
            };
        }
    }
}
