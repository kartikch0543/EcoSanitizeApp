using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class DisposalController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public DisposalController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index(string? search, string? category, string? priority, string? status, int page = 1)
        {
            var requests = _sanitizationService.GetAllDisposalRequests();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                requests = requests.Where(r => r.Location.ToLower().Contains(q) || (r.AssignedCollector != null && r.AssignedCollector.ToLower().Contains(q))).ToList();
                ViewData["SearchQuery"] = search;
            }

            if (!string.IsNullOrEmpty(category))
            {
                requests = requests.Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveCategory"] = category;
            }

            if (!string.IsNullOrEmpty(priority))
            {
                requests = requests.Where(r => r.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActivePriority"] = priority;
            }

            if (!string.IsNullOrEmpty(status))
            {
                requests = requests.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveStatus"] = status;
            }

            const int pageSize = 8;
            int totalCount = requests.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var pagedRequests = requests.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(pagedRequests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new DisposalRequest
            {
                RequestDate = DateTime.Now,
                Priority = "Medium"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DisposalRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _sanitizationService.AddDisposalRequest(model);
            TempData["SuccessMessage"] = $"Collection request #{model.Id} at {model.Location} has been registered.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var request = _sanitizationService.GetDisposalRequestById(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = $"Disposal request #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            WasteItemBase wasteItem = request.Category switch
            {
                "Organic" => new OrganicWaste(request.Id, request.Category, request.EstimatedWeightKg, request.Location),
                "Hazardous" => new HazardousWaste(request.Id, request.Category, request.EstimatedWeightKg, request.Location),
                "Sanitary" => new SanitaryWaste(request.Id, request.Category, request.EstimatedWeightKg, request.Location),
                "E-Waste" => new ElectronicWaste(request.Id, request.Category, request.EstimatedWeightKg, request.Location),
                _ => new RecyclableWaste(request.Id, request.Category, request.EstimatedWeightKg, request.Location)
            };

            ViewBag.BinColor = wasteItem.GetBinColor();
            ViewBag.HandlingInstructions = wasteItem.GetHandlingInstructions();
            ViewBag.TargetFacility = wasteItem.GetTargetFacility();
            ViewBag.HazardRating = wasteItem.GetHazardRating();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status, string? collector)
        {
            bool updated = _sanitizationService.UpdateDisposalStatus(id, status, collector);
            if (updated)
            {
                TempData["SuccessMessage"] = $"Request #{id} status updated to {status}.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Disposal request #{id} could not be updated.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            bool deleted = _sanitizationService.DeleteDisposalRequest(id);
            if (deleted)
            {
                TempData["SuccessMessage"] = $"Request #{id} has been removed from active queue.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
