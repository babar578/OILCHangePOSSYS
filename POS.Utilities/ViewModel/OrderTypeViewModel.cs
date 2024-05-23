using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator OrderTypeViewModel(OrderType type)
        {
            if (type == null)
                return null;

            return new OrderTypeViewModel()
            {
                Id = type.Id,
                Name = type.Name,
            };
        }

        public static implicit operator OrderType(OrderTypeViewModel type)
        {
            if (type == null)
                return null;

            return new OrderType()
            {
                Id = type.Id,
                Name = type.Name,
            };
        }
    }
}
