using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public ScheduleController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index(string? day, string? zone, string? stream)
        {
            var schedules = _sanitizationService.GetAllSchedules();

            if (!string.IsNullOrWhiteSpace(day))
            {
                schedules = schedules.Where(s => s.CollectionDay.Equals(day, System.StringComparison.OrdinalIgnoreCase) || s.CollectionDay.Equals("Daily", System.StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveDay"] = day;
            }

            if (!string.IsNullOrWhiteSpace(zone))
            {
                schedules = schedules.Where(s => s.WardZone.Equals(zone, System.StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveZone"] = zone;
            }

            if (!string.IsNullOrWhiteSpace(stream))
            {
                schedules = schedules.Where(s => s.WasteStream.Equals(stream, System.StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveStream"] = stream;
            }

            ViewBag.Workers = _sanitizationService.GetAllWorkers().ToList();
            return View(schedules);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Workers = _sanitizationService.GetAllWorkers().ToList();
            return View(new CollectionSchedule());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CollectionSchedule model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Workers = _sanitizationService.GetAllWorkers().ToList();
                return View(model);
            }

            _sanitizationService.AddSchedule(model);
            TempData["SuccessMessage"] = $"Scheduled collection route for {model.WardZone} on {model.CollectionDay} registered.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id)
        {
            _sanitizationService.ToggleScheduleStatus(id);
            TempData["SuccessMessage"] = "Collection schedule status toggled.";
            return RedirectToAction(nameof(Index));
        }
    }
}
