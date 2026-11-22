using System;
using System.ComponentModel.DataAnnotations;

namespace EcoSanitizeApp.Models
{
    public class SanitizationAudit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Facility or area name is required.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Facility name must be between 3 and 80 characters.")]
        [Display(Name = "Inspected Area or Facility")]
        public string AreaName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cleanliness score is required.")]
        [Range(1, 5, ErrorMessage = "Cleanliness score must be between 1 and 5.")]
        [Display(Name = "Cleanliness Score (1 to 5)")]
        public int CleanlinessScore { get; set; }

        [Required(ErrorMessage = "Disinfectant used is required.")]
        [Display(Name = "Disinfectant Applied")]
        public string DisinfectantUsed { get; set; } = string.Empty;

        [Display(Name = "Inspection Date")]
        public DateTime AuditDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Inspector name is required.")]
        [Display(Name = "Inspector Name")]
        public string InspectorName { get; set; } = string.Empty;

        [Display(Name = "Requires Follow-up Cleaning")]
        public bool RequiresFollowUp { get; set; }

        [Display(Name = "Inspection Outcome")]
        public string ResultStatus { get; set; } = "Pass";
    }
}
