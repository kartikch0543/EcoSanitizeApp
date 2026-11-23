using System;
using System.ComponentModel.DataAnnotations;

namespace EcoSanitizeApp.Models
{
    public class SanitationWorker
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Worker code is required.")]
        [Display(Name = "Employee Code")]
        public string WorkerCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Worker name is required.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 80 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Contact Phone")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Assigned municipal zone is required.")]
        [Display(Name = "Assigned Zone")]
        public string AssignedZone { get; set; } = "Zone 1 - North Market";

        [Required(ErrorMessage = "Specialization is required.")]
        [Display(Name = "Specialization / Role")]
        public string Specialization { get; set; } = "Waste Collection Driver";

        [Display(Name = "Duty Status")]
        public string Status { get; set; } = "Active";

        [Display(Name = "Assigned Vehicle / Equipment")]
        public string? AssignedVehicle { get; set; }

        [Display(Name = "Date Joined")]
        public DateTime DateJoined { get; set; } = DateTime.Now;
    }
}
