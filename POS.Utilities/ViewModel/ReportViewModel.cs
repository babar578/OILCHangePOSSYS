using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ReportViewModel
    {
        public string RdlcFileName { get; set; }
        public string ReportFilePath { get; set; }
        public string DatasetName { get; set; }
        public DataSet Dataset { get; set; }
    }
}
