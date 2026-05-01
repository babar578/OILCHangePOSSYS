using POS.Utilities.Services;
using POS.Utilities.ViewModel;
using System;
using System.Web.Mvc;

namespace POS.Web.Controllers
{
    /// <summary>
    /// Controller for public website and lead management
    /// </summary>
    public class WebsiteController : Controller
    {
        /// <summary>
        /// Single-page public website
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// API endpoint for lead submission
        /// POST /api/leads
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SubmitLead(WebsiteLeadViewModel model)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(model.FullName))
                {
                    return Json(new { success = false, message = "Full Name is required." });
                }

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    return Json(new { success = false, message = "Email is required." });
                }

                if (string.IsNullOrWhiteSpace(model.Message))
                {
                    return Json(new { success = false, message = "Message is required." });
                }

                // Validate email format
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.Email, 
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    return Json(new { success = false, message = "Invalid email format." });
                }

                // Set defaults
                model.Id = Guid.NewGuid();
                model.Status = "New";
                model.Source = "Web";
                model.Language = "en";
                model.CreatedAt = DateTime.UtcNow;
                model.IsActive = true;

                // Save lead
                bool result = LeadService.CreateLead(model);

                if (result)
                {
                    return Json(new { success = true, message = "Thank you! We'll contact you soon." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to submit your query. Please try again." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error submitting lead: {ex.Message}");
                return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
        }

        /// <summary>
        /// Alternative API endpoint without anti-forgery token (for external forms)
        /// POST /api/leads/public
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult SubmitLeadPublic(WebsiteLeadViewModel model)
        {
            return SubmitLead(model);
        }
    }
}

