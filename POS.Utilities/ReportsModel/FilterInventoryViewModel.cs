using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ReportsModel
{
    public class FilterInventoryViewModel : DateRangeViewModel
    {
        public int? ItemId { get; set; }
        public List<ItemViewModel> Items
        {
            get
            {
                return ItemServices.GetAllItems(true);
            }
        }

        public int? UnitId { get; set; }
        public List<UnitViewModel> Units
        {
            get
            {
                return ItemServices.GetAllUnits();
            }

        }

    }
}
