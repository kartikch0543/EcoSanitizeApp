using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Models;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public HomeController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index()
        {
            var summary = _sanitizationService.GetDashboardSummary();
            ViewBag.RecentRequests = _sanitizationService.GetAllDisposalRequests().Take(5).ToList();
            ViewBag.RecentAudits = _sanitizationService.GetAllSanitizationAudits().Take(3).ToList();
            ViewBag.RecentComplaints = _sanitizationService.GetAllComplaints().Take(4).ToList();

            return View(summary);
        }

        public IActionResult SegregationGuide()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
