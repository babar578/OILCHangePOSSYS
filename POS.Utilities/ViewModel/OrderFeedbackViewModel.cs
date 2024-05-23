using POS.Database.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Utilities.ViewModel
{
    public class OrderFeedbackViewModel
    {
        public OrderFeedbackViewModel()
        {
            OrderFeedbackStatuses = new List<OrderFeedbackStatusViewModel>();
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> FoodExcellence { get; set; }
        public Nullable<int> ServiceLevel { get; set; }
        public Nullable<int> AmbienceTheme { get; set; }
        public Nullable<int> SelectionOfMusic { get; set; }
        public Nullable<int> OverallExperience { get; set; }
        public string Comments { get; set; }
        public string GuestName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailMarketing { get; set; }
        public bool IsSmsMarketing { get; set; }
        public bool IsWhatsAppMarketing { get; set; }
        public bool IsPhoneMarketing { get; set; }
        public OrderViewModel Order { get; set; }

        public List<OrderFeedbackStatusViewModel> OrderFeedbackStatuses { get; set; }

        public static implicit operator OrderFeedbackViewModel(OrderFeedback feedback)
        {
            if (feedback == null)
                return null;

            return new OrderFeedbackViewModel()
            {
                Id = feedback.Id,
                Comments = feedback.Comments,
                ContactNumber = feedback.ContactNumber,
                Email = feedback.Email,
                GuestName = feedback.GuestName,
                IsEmailMarketing = feedback.IsEmailMarketing,
                IsPhoneMarketing = feedback.IsPhoneMarketing,
                IsSmsMarketing = feedback.IsSmsMarketing,
                IsWhatsAppMarketing = feedback.IsWhatsAppMarketing,
                OrderId = feedback.OrderId,
                AmbienceTheme = feedback.AmbienceTheme,
                FoodExcellence = feedback.FoodExcellence,
                OverallExperience = feedback.OverallExperience,
                SelectionOfMusic = feedback.SelectionOfMusic,
                ServiceLevel = feedback.SelectionOfMusic,
            };
        }

        public static implicit operator OrderFeedback(OrderFeedbackViewModel feedback)
        {
            if (feedback == null)
                return null;

            return new OrderFeedback()
            {
                Id = feedback.Id,
                Comments = feedback.Comments,
                ContactNumber = feedback.ContactNumber,
                Email = feedback.Email,
                GuestName = feedback.GuestName,
                IsEmailMarketing = feedback.IsEmailMarketing,
                IsPhoneMarketing = feedback.IsPhoneMarketing,
                IsSmsMarketing = feedback.IsSmsMarketing,
                IsWhatsAppMarketing = feedback.IsWhatsAppMarketing,
                OrderId = feedback.OrderId,
                AmbienceTheme = feedback.AmbienceTheme,
                FoodExcellence = feedback.FoodExcellence,
                OverallExperience = feedback.OverallExperience,
                SelectionOfMusic = feedback.SelectionOfMusic,
                ServiceLevel = feedback.SelectionOfMusic,
            };
        }

    }
}
