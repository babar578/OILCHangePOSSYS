using System;

namespace POS.Utilities.ViewModel
{
    public class WebsiteLeadViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string InterestedPlan { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public int? AssignedTo { get; set; }
        public string Notes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}

