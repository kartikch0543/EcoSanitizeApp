using System;
using System.ComponentModel.DataAnnotations;

namespace EcoSanitizeApp.Models
{
    public class SanitationComplaint
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Complaint title is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
        [Display(Name = "Complaint Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please describe the issue.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be at least 10 characters.")]
        [Display(Name = "Detailed Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Problem Category")]
        public string Category { get; set; } = "Overflowing Bin";

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 120 characters.")]
        [Display(Name = "Incident Location")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ward / Zone is required.")]
        [Display(Name = "Ward / Zone")]
        public string WardZone { get; set; } = "Zone 1 - North Market";

        [Required(ErrorMessage = "Severity level is required.")]
        [Display(Name = "Severity")]
        public string Severity { get; set; } = "Medium";

        [Display(Name = "Current Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Evidence Photo")]
        public string? PhotoPath { get; set; }

        [Required(ErrorMessage = "Your name is required.")]
        [Display(Name = "Reported By")]
        public string ReportedBy { get; set; } = "Citizen";

        [Display(Name = "Contact Phone")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? ContactNumber { get; set; }

        [Display(Name = "Reported Date")]
        public DateTime ReportedDate { get; set; } = DateTime.Now;

        [Display(Name = "Assigned Sanitation Worker")]
        public string? AssignedWorker { get; set; }

        [Display(Name = "Resolution Notes")]
        public string? ResolutionNotes { get; set; }

        [Display(Name = "Resolved Timestamp")]
        public DateTime? ResolvedDate { get; set; }
    }
}
