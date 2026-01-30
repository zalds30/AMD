using AMD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AMD.Pages
{
    public class BookingModel : PageModel
    {
        [BindProperty]
        public BookingRequest Booking { get; set; }

        // Available event types for dropdown
        public List<string> EventTypes { get; set; } = new List<string>
        {
            "Wedding",
            "Birthday Party",
            "Corporate Event",
            "Concert/Show",
            "Private Party",
            "Conference",
            "School Event",
            "Church Event",
            "Other"
        };

        // Available services for checkboxes
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

        // Budget ranges for dropdown
        public List<BudgetRange> BudgetRanges { get; set; } = new List<BudgetRange>
        {
            new BudgetRange { Value = "5000-15000", Display = "₱5,000 - ₱15,000" },
            new BudgetRange { Value = "15000-30000", Display = "₱15,000 - ₱30,000" },
            new BudgetRange { Value = "30000-50000", Display = "₱30,000 - ₱50,000" },
            new BudgetRange { Value = "50000-100000", Display = "₱50,000 - ₱100,000" },
            new BudgetRange { Value = "100000+", Display = "₱100,000+" },
            new BudgetRange { Value = "custom", Display = "Custom Budget" }
        };

        public void OnGet()
        {
            // Initialize with today's date + 7 days as default
            Booking = new BookingRequest
            {
                EventDate = DateTime.Today.AddDays(7),
                EventTime = TimeSpan.FromHours(18) // Default 6:00 PM
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Here you would save to database, send emails, etc.
                // For now, we'll just show a success message

                TempData["SuccessMessage"] = $"Thank you {Booking.FullName}! Your booking request has been submitted. We'll contact you within 24 hours.";

                // In a real application:
                // 1. Save to database
                // 2. Send confirmation email
                // 3. Send notification to admin
                // 4. Generate quote if needed

                // Log the booking
                Console.WriteLine($"New booking: {Booking.FullName} - {Booking.EventType} - {Booking.EventDate:d}");

                // Redirect to confirmation page or show success message
                return RedirectToPage("/BookingSuccess", new
                {
                    name = Booking.FullName,
                    date = Booking.EventDate.ToString("yyyy-MM-dd")
                });

                // Or stay on page with success message:
                // return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return Page();
            }
        }
    }

    // Helper classes
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