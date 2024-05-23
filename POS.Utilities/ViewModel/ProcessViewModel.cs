using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class ProcessViewModel
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public Nullable<System.DateTime> TransationDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator ProcessViewModel(Process process)
        {
            if (process == null)
                return null;

            return new ProcessViewModel()
            {
                Id = process.Id,
                Payment = process.Payment,
                TransationDate = process.TransationDate,
                CreationDate = process.CreationDate,
                ModificationDate = process.ModificationDate,
                IsActive = process.IsActive,
            };
        }

        public static implicit operator Process(ProcessViewModel process)
        {
            if (process == null)
                return null;

            return new Process()
            {
                Id = process.Id,
                Payment = process.Payment,
                TransationDate = process.TransationDate,
                CreationDate = process.CreationDate,
                ModificationDate = process.ModificationDate,
                IsActive = process.IsActive
            };
        }
    }
}


