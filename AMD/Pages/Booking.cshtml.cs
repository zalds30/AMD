using AMD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMD.Pages
{
    public class BookingModel : PageModel
    {
        [BindProperty]
        public BookingRequest Booking { get; set; }

        public List<string> EventTypes { get; set; } = new List<string>
        {
            "Wedding", "Birthday Party", "Corporate Event", "Concert/Show",
            "Private Party", "Conference", "School Event", "Church Event", "Other"
        };

        public List<ServiceItem> AvailableServices { get; set; } = new List<ServiceItem>
        {
            new ServiceItem { Id = "audio", Name = "Audio System", Icon = "fa-volume-up" },
            new ServiceItem { Id = "lighting", Name = "Lighting", Icon = "fa-lightbulb" },
            new ServiceItem { Id = "dj", Name = "DJ Services", Icon = "fa-music" },
            new ServiceItem { Id = "streaming", Name = "Live Streaming", Icon = "fa-video" },
            new ServiceItem { Id = "stage", Name = "Stage Setup", Icon = "fa-theater-masks" },
            new ServiceItem { Id = "mics", Name = "Microphones", Icon = "fa-microphone" },
            new ServiceItem { Id = "backline", Name = "Backline Equipment", Icon = "fa-guitar" }
        };

        public List<BudgetRange> BudgetRanges { get; set; } = new List<BudgetRange>
        {
            new BudgetRange { Value = "5000-15000", Display = "₱5,000 - ₱15,000" },
            new BudgetRange { Value = "15000-30000", Display = "₱15,000 - ₱30,000" },
            new BudgetRange { Value = "30000-50000", Display = "₱30,000 - ₱50,000" },
            new BudgetRange { Value = "50000-100000", Display = "₱50,000 - ₱100,000" },
            new BudgetRange { Value = "100000+", Display = "₱100,000+" },
            new BudgetRange { Value = "custom", Display = "Custom Budget" }
        };

        public List<string> ReferralSources { get; set; } = new List<string>
        {
            "Facebook", "Instagram", "Friend/Family Referral",
            "Google Search", "Previous Customer", "Flyer/Poster", "Other"
        };

        public void OnGet()
        {
            Booking = new BookingRequest
            {
                EventDate = DateTime.Today.AddDays(7),
                EventTime = TimeSpan.FromHours(18),
                NeedsSetupTime = true,
                NeedsSoundCheck = true,
                Status = "Pending"
            };

            // Initialize SelectedServices dictionary
            foreach (var service in AvailableServices)
            {
                Booking.SelectedServices[service.Name] = false;
            }
        }

        public IActionResult OnPost()
        {
            // Build RequiredServices list from SelectedServices
            Booking.RequiredServices = Booking.SelectedServices
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ✅ FIXED: Safe way to format TimeSpan to time string
            string eventTimeString = "TBD";
            if (Booking.EventTime.HasValue)
            {
                try
                {
                    // Convert TimeSpan to DateTime for proper time formatting
                    var timeOfDay = DateTime.Today.Add(Booking.EventTime.Value);
                    eventTimeString = timeOfDay.ToString("hh:mm tt");
                }
                catch
                {
                    eventTimeString = Booking.EventTime.Value.ToString(@"hh\:mm");
                }
            }

            // Save to TempData
            TempData["MessengerLink"] = "https://m.me/996504260221621";
            TempData["BookingName"] = Booking.FullName;
            TempData["BookingDate"] = Booking.EventDate.ToString("MMMM dd, yyyy");

            var requiredServices = Booking.RequiredServices != null && Booking.RequiredServices.Any()
                ? string.Join(", ", Booking.RequiredServices)
                : "None";

            // ✅ FIXED: Removed problematic TimeSpan formatting
            TempData["BookingMessage"] = $@"━━━━━━━━━━━━━━━━━━━━
📝 CUSTOMER DETAILS
━━━━━━━━━━━━━━━━━━━━
Name: {Booking.FullName}
Phone: {Booking.Phone}
Email: {Booking.Email}

━━━━━━━━━━━━━━━━━━━━
🎉 EVENT DETAILS
━━━━━━━━━━━━━━━━━━━━
Event: {Booking.EventType}
Date: {Booking.EventDate:MMMM dd, yyyy}
Time: {eventTimeString}
Location: {Booking.Location}
Guests: {(Booking.EstimatedGuests.HasValue ? Booking.EstimatedGuests.Value.ToString("N0") : "Not specified")}
Duration: {(Booking.DurationHours.HasValue ? Booking.DurationHours.Value + " hours" : "Not specified")}

━━━━━━━━━━━━━━━━━━━━
💰 BUDGET & SERVICES
━━━━━━━━━━━━━━━━━━━━
Budget: {(string.IsNullOrEmpty(Booking.BudgetRange) ? "Not specified" : Booking.BudgetRange)}
Services: {requiredServices}

━━━━━━━━━━━━━━━━━━━━
📝 ADDITIONAL DETAILS
━━━━━━━━━━━━━━━━━━━━
{(!string.IsNullOrEmpty(Booking.AdditionalDetails) ? Booking.AdditionalDetails : "No additional details provided")}

━━━━━━━━━━━━━━━━━━━━
📢 Referral Source: {(string.IsNullOrEmpty(Booking.ReferralSource) ? "Not specified" : Booking.ReferralSource)}
⏰ Setup Time Required: {(Booking.NeedsSetupTime ? "Yes" : "No")}
🎵 Sound Check Required: {(Booking.NeedsSoundCheck ? "Yes" : "No")}

━━━━━━━━━━━━━━━━━━━━
⏰ Submitted: {DateTime.Now:MMMM dd, yyyy hh:mm tt}";

            return RedirectToPage("/BookingSuccess");
        }
    }

    public class ServiceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class BudgetRange
    {
        public string Value { get; set; }
        public string Display { get; set; }
    }
}