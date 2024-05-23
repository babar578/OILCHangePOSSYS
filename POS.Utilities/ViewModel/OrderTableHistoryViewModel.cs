using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderTableHistoryViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TableId { get; set; }
        public System.DateTime CreationDate { get; set; }

        public static implicit operator OrderTableHistoryViewModel(OrderTableHistory history)
        {
            if (history == null)
                return null;

            return new OrderTableHistoryViewModel()
            {
                Id = history.Id,
                OrderId = history.OrderId,
                TableId = history.TableId,
                CreationDate = history.CreationDate,
            };
        }

        public static implicit operator OrderTableHistory(OrderTableHistoryViewModel history)
        {
            if (history == null)
                return null;

            return new OrderTableHistory()
            {
                Id = history.Id,
                OrderId = history.OrderId,
                TableId = history.TableId,
                CreationDate = history.CreationDate,
            };
        }
    }
}
