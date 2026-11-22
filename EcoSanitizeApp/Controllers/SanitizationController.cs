using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class SanitizationController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public SanitizationController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index()
        {
            var audits = _sanitizationService.GetAllSanitizationAudits();
            ViewData["TotalAudits"] = audits.Count();
            ViewData["FollowUpCount"] = audits.Count(a => a.RequiresFollowUp);

            return View(audits);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SanitizationAudit
            {
                AuditDate = DateTime.Now,
                CleanlinessScore = 4
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SanitizationAudit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _sanitizationService.LogSanitizationAudit(model);
            TempData["SuccessMessage"] = $"Sanitization audit for {model.AreaName} logged with rating {model.CleanlinessScore}/5.";

            return RedirectToAction(nameof(Index));
        }
    }
}
