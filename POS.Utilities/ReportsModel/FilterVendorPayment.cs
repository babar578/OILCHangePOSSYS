using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ReportsModel
{
    public class FilterVendorPayment : DateRangeViewModel
    {
        public int? VendorId { get; set; }
        public List<VendorViewModel> Vendors
        {
            get
            {
                return VendorServices.GetAllVendors();
            }
        }
    }
}
