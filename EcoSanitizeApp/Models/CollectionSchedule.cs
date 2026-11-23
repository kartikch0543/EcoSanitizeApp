using System;
using System.ComponentModel.DataAnnotations;

namespace EcoSanitizeApp.Models
{
    public class CollectionSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Zone is required.")]
        [Display(Name = "Ward / Zone")]
        public string WardZone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Collection day is required.")]
        [Display(Name = "Collection Day")]
        public string CollectionDay { get; set; } = "Monday";

        [Required(ErrorMessage = "Time slot is required.")]
        [Display(Name = "Operating Time Slot")]
        public string TimeSlot { get; set; } = "06:00 - 09:00";

        [Required(ErrorMessage = "Waste stream is required.")]
        [Display(Name = "Designated Waste Stream")]
        public string WasteStream { get; set; } = "Organic";

        [Required(ErrorMessage = "Assigned vehicle is required.")]
        [Display(Name = "Collection Vehicle")]
        public string AssignedVehicle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Assigned worker is required.")]
        [Display(Name = "Lead Worker / Driver")]
        public string AssignedWorker { get; set; } = string.Empty;

        [Display(Name = "Route Details")]
        public string? RouteDescription { get; set; }

        [Display(Name = "Active Schedule")]
        public bool IsActive { get; set; } = true;
    }
}
