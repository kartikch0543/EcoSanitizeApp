using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public WorkerController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index(string? status, string? zone, string? search)
        {
            var workers = _sanitizationService.GetAllWorkers();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                workers = workers.Where(w => w.FullName.ToLower().Contains(q) || w.WorkerCode.ToLower().Contains(q) || w.Specialization.ToLower().Contains(q)).ToList();
                ViewData["SearchQuery"] = search;
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                workers = workers.Where(w => w.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveStatus"] = status;
            }

            if (!string.IsNullOrWhiteSpace(zone))
            {
                workers = workers.Where(w => w.AssignedZone.Equals(zone, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveZone"] = zone;
            }

            ViewBag.TotalCount = workers.Count();
            ViewBag.ActiveCount = workers.Count(w => w.Status == "Active");
            ViewBag.OnDutyCount = workers.Count(w => w.Status == "On-Duty");

            return View(workers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var nextCodeNumber = _sanitizationService.GetAllWorkers().Count() + 101;
            var model = new SanitationWorker
            {
                WorkerCode = $"WRK-{nextCodeNumber}",
                Status = "Active",
                DateJoined = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SanitationWorker model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _sanitizationService.AddWorker(model);
            TempData["SuccessMessage"] = $"Sanitation worker {model.FullName} ({model.WorkerCode}) has been registered.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status, string? zone)
        {
            bool updated = _sanitizationService.UpdateWorkerStatus(id, status, zone);
            if (updated)
            {
                TempData["SuccessMessage"] = "Worker duty status updated successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
