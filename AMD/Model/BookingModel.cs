using System;
using System.ComponentModel.DataAnnotations;

namespace AMD.Models
{
    public class BookingRequest
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        [FutureDate(ErrorMessage = "Event date must be in the future")]
        public DateTime EventDate { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Event Time")]
        public TimeSpan? EventTime { get; set; }

        [Required(ErrorMessage = "Event type is required")]
        [Display(Name = "Event Type")]
        public string EventType { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot be longer than 200 characters")]
        [Display(Name = "Event Location")]
        public string Location { get; set; }

        [Display(Name = "Estimated Guests")]
        [Range(1, 10000, ErrorMessage = "Number of guests must be between 1 and 10,000")]
        public int? EstimatedGuests { get; set; }

        [Display(Name = "Event Duration (hours)")]
        [Range(1, 24, ErrorMessage = "Duration must be between 1 and 24 hours")]
        public int? DurationHours { get; set; }

        [Display(Name = "Required Services")]
        public List<string> RequiredServices { get; set; } = new List<string>();

        [Display(Name = "Budget Range")]
        public string BudgetRange { get; set; }

        [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters")]
        [Display(Name = "Additional Details")]
        public string AdditionalDetails { get; set; }

        [Display(Name = "How did you hear about us?")]
        public string ReferralSource { get; set; }

        [Display(Name = "Setup Time Required")]
        public bool NeedsSetupTime { get; set; } = true;

        [Display(Name = "Sound Check Required")]
        public bool NeedsSoundCheck { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";
    }

    // Custom Validation Attribute for future date
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }
            return false;
        }
    }

    // Model for Equipment Rental
    public class EquipmentRental
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Range(0.01, 100000)]
        [DataType(DataType.Currency)]
        public decimal DailyRate { get; set; }

        [Range(0.01, 100000)]
        [DataType(DataType.Currency)]
        public decimal WeeklyRate { get; set; }

        public int QuantityAvailable { get; set; }

        [StringLength(50)]
        public string Condition { get; set; } = "Excellent";

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        public DateTime LastMaintenance { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Model for Contact Form
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public string Status { get; set; } = "New";
    }
}