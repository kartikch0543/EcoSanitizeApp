using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly ISanitizationService _sanitizationService;
        private readonly IWebHostEnvironment _environment;

        public ComplaintController(ISanitizationService sanitizationService, IWebHostEnvironment environment)
        {
            _sanitizationService = sanitizationService;
            _environment = environment;
        }

        public IActionResult Index(string? search, string? category, string? status, string? severity, int page = 1)
        {
            var complaints = _sanitizationService.GetAllComplaints();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower();
                complaints = complaints.Where(c => c.Title.ToLower().Contains(q) || c.Location.ToLower().Contains(q) || c.ReportedBy.ToLower().Contains(q)).ToList();
                ViewData["SearchQuery"] = search;
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                complaints = complaints.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveCategory"] = category;
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                complaints = complaints.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveStatus"] = status;
            }

            if (!string.IsNullOrWhiteSpace(severity))
            {
                complaints = complaints.Where(c => c.Severity.Equals(severity, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["ActiveSeverity"] = severity;
            }

            const int pageSize = 8;
            int totalCount = complaints.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var pagedComplaints = complaints.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.Workers = _sanitizationService.GetAllWorkers().Where(w => w.Status != "On-Leave").ToList();

            return View(pagedComplaints);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SanitationComplaint
            {
                ReportedDate = DateTime.Now,
                Severity = "Medium"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SanitationComplaint model, IFormFile? photoFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Handle image upload if provided
            if (photoFile != null && photoFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();

                if (allowedExtensions.Contains(extension) && photoFile.Length <= 5 * 1024 * 1024)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "complaints");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"complaint_{DateTime.UtcNow.Ticks}{extension}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photoFile.CopyTo(stream);
                    }

                    model.PhotoPath = $"/uploads/complaints/{uniqueFileName}";
                }
            }

            _sanitizationService.AddComplaint(model);
            TempData["SuccessMessage"] = $"Sanitation grievance #{model.Id} ('{model.Title}') has been logged successfully.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var complaint = _sanitizationService.GetComplaintById(id);
            if (complaint == null)
            {
                TempData["ErrorMessage"] = $"Sanitation complaint #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Workers = _sanitizationService.GetAllWorkers().Where(w => w.Status != "On-Leave").ToList();
            return View(complaint);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status, string? assignedWorker, string? resolutionNotes)
        {
            bool updated = _sanitizationService.UpdateComplaintStatus(id, status, assignedWorker, resolutionNotes);
            if (updated)
            {
                TempData["SuccessMessage"] = $"Complaint #{id} status updated to '{status}'.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Unable to update complaint #{id}.";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
