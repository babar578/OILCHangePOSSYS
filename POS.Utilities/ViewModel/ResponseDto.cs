using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ResponseDto
    {


        public bool Status { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        // To concatinate the employeeId with image and document path
        public int EmployeeID { get; set; }
        // To Select the record in database by that table's unique key
        public int ID { get; set; }
        public decimal Val { get; set; }
        public Boolean IsVal { get; set; }

        public decimal Val1 { get; set; }

        //public List<string> ModelErrors { get; set; }

        // Custom
        //Used in supplier and buyer screen when we save them as buyer or supplier
        public string CustomMessage { get; set; }
        public string CodeID { get; set; }
        public int PrintId { get; set; }


    }
}
