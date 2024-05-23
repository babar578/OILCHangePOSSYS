using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderFeedbackStatusViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public static implicit operator OrderFeedbackStatusViewModel(OrderFeedbackStatus status)
        {
            if (status == null)
                return null;

            return new OrderFeedbackStatusViewModel()
            {
                Id = status.Id,
                Name = status.Name,
                IsActive = status.IsActive,
            };
        }

        public static implicit operator OrderFeedbackStatus(OrderFeedbackStatusViewModel status)
        {
            if (status == null)
                return null;

            return new OrderFeedbackStatus()
            {
                Id = status.Id,
                Name = status.Name,
                IsActive = status.IsActive,
            };
        }

    }
}
