using POS.Database.DatabaseModel;
using POS.Utilities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class VendorPaymentViewModel
    {
        public VendorPaymentViewModel()
        {

            Vendors = new List<VendorViewModel>();
            VendorPaymentTypes = new List<VendorPaymentTypeViewModel>();

        }
        public int Id { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public int VendorId { get; set; }
        public string DocNo { get; set; }
        public int PaymentTypeId { get; set; }
        public double Payment { get; set; }
        public Nullable<double> RemainingBalance { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }

        public VendorPaymentTypeViewModel VendorPaymentType { get; set; }
        public VendorViewModel Vendor { get; set; }

        public List<VendorPaymentTypeViewModel> VendorPaymentTypes { get; set; }
        public List<VendorViewModel> Vendors { get; set; }

        public string VendorName
        {
            get
            {
                if (VendorId > 0)
                {
                    return VendorServices.GetVendorNameById(VendorId);
                }
                else
                {
                    return " - ";
                }
            }
        }


        public string PaymentTypeName
        {
            get
            {
                if (PaymentTypeId > 0)
                {
                    return VendorServices.GetVendorPaymentTypeNameById(PaymentTypeId);
                }
                else
                {
                    return " - ";
                }
            }
        }

        public string CreatedByName
        {
            get
            {
                if (CreatedBy > 0)
                {
                    return UserServices.GetUserNameById(CreatedBy);
                }
                else
                {
                    return " - ";
                }
            }
        }



        public static implicit operator VendorPaymentViewModel(VendorPayment payment)
        {
            if (payment == null)
                return null;

            return new VendorPaymentViewModel()
            {
                Id = payment.Id,
                CreatedBy = payment.CreatedBy,
                CreationDate = payment.CreationDate,
                DocNo = payment.DocNo,
                ModifyDate = payment.ModifyDate,
                Payment = payment.Payment,
                RemainingBalance = payment.RemainingBalance,
                Remarks = payment.Remarks,
                TransactionDate = payment.TransactionDate,
                VendorId = payment.VendorId,
                PaymentTypeId = payment.PaymentTypeId,
            };
        }

        public static implicit operator VendorPayment(VendorPaymentViewModel payment)
        {
            if (payment == null)
                return null;

            return new VendorPayment()
            {
                Id = payment.Id,
                CreatedBy = payment.CreatedBy,
                CreationDate = payment.CreationDate,
                DocNo = payment.DocNo,
                ModifyDate = payment.ModifyDate,
                Payment = payment.Payment,
                RemainingBalance = payment.RemainingBalance,
                Remarks = payment.Remarks,
                TransactionDate = payment.TransactionDate,
                VendorId = payment.VendorId,
                PaymentTypeId = payment.PaymentTypeId,
            };
        }

    }
}
