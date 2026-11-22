using System;
using System.ComponentModel.DataAnnotations;

namespace EcoSanitizeApp.Models
{
    public class DisposalRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 100 characters.")]
        [Display(Name = "Collection Location")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a waste segregation category.")]
        [Display(Name = "Segregation Stream")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Estimated weight is required.")]
        [Range(0.1, 500.0, ErrorMessage = "Estimated weight must be between 0.1 kg and 500.0 kg.")]
        [Display(Name = "Estimated Weight (kg)")]
        public double EstimatedWeightKg { get; set; }

        [Required(ErrorMessage = "Priority level is required.")]
        [Display(Name = "Priority Level")]
        public string Priority { get; set; } = "Medium";

        [Display(Name = "Collection Status")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "Logged Timestamp")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "Assigned Driver / Unit")]
        public string? AssignedCollector { get; set; }

        [StringLength(250, ErrorMessage = "Special instructions cannot exceed 250 characters.")]
        [Display(Name = "Special Handling Notes")]
        public string? SpecialNotes { get; set; }
    }
}
