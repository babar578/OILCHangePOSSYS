using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class PrintInfoViewModel
    {
        public int Id { get; set; }
        public string PrinterName { get; set; }
        public string IP_Address { get; set; }
        public int Port { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentViewModel Department { get; set; }

        public static implicit operator PrintInfoViewModel(PrintInfo print)
        {
            if (print == null)
                return null;

            return new PrintInfoViewModel()
            {
                Id = print.Id,
                DepartmentId = print.DepartmentId,
                IP_Address = print.IP_Address,
                Port = print.Port,
                PrinterName = print.PrinterName,
            };
        }

        public static implicit operator PrintInfo(PrintInfoViewModel print)
        {
            if (print == null)
                return null;

            return new PrintInfo()
            {
                Id = print.Id,
                DepartmentId = print.DepartmentId,
                IP_Address = print.IP_Address,
                Port = print.Port,
                PrinterName = print.PrinterName,
            };
        }
    }
}
