using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Services;

namespace EcoSanitizeApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly ISanitizationService _sanitizationService;

        public AnalyticsController(ISanitizationService sanitizationService)
        {
            _sanitizationService = sanitizationService;
        }

        public IActionResult Index()
        {
            var summary = _sanitizationService.GetDashboardSummary();
            var requests = _sanitizationService.GetAllDisposalRequests().ToList();
            var complaints = _sanitizationService.GetAllComplaints().ToList();
            var audits = _sanitizationService.GetAllSanitizationAudits().ToList();

            // Stream breakdowns
            var streamGroups = requests.GroupBy(r => r.Category)
                .Select(g => new { Stream = g.Key, Weight = Math.Round(g.Sum(x => x.EstimatedWeightKg), 1), Count = g.Count() })
                .ToList();

            ViewBag.StreamLabels = streamGroups.Select(s => s.Stream).ToArray();
            ViewBag.StreamWeights = streamGroups.Select(s => s.Weight).ToArray();

            // Status breakdowns
            ViewBag.StatusLabels = new[] { "Pending", "In-Transit", "Disposed" };
            ViewBag.StatusCounts = new[] { summary.PendingRequests, summary.InTransitRequests, summary.DisposedRequests };

            // Complaint category breakdowns
            var complaintCats = complaints.GroupBy(c => c.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();
            ViewBag.ComplaintCatLabels = complaintCats.Select(c => c.Category).ToArray();
            ViewBag.ComplaintCatCounts = complaintCats.Select(c => c.Count).ToArray();

            // Daily trend (simulated aggregation over last 7 days)
            var days = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-6 + i)).ToList();
            ViewBag.TrendDates = days.Select(d => d.ToString("MMM dd")).ToArray();
            ViewBag.TrendWeights = days.Select(d =>
            {
                var dayRequests = requests.Where(r => r.RequestDate.Date == d.Date).ToList();
                return dayRequests.Any() ? Math.Round(dayRequests.Sum(r => r.EstimatedWeightKg), 1) : Math.Round(20.0 + (d.Day % 5) * 15.0, 1);
            }).ToArray();

            ViewBag.Summary = summary;
            ViewBag.TotalAudits = audits.Count;
            ViewBag.CompliantAudits = audits.Count(a => !a.RequiresFollowUp);

            return View(summary);
        }

        // CSV Export for Disposal Requests
        public IActionResult ExportDisposalCsv()
        {
            var requests = _sanitizationService.GetAllDisposalRequests();
            var sb = new StringBuilder();
            sb.AppendLine("Id,Location,Stream,EstimatedWeightKg,Priority,Status,AssignedCollector,RequestDate,SpecialNotes");

            foreach (var r in requests)
            {
                string cleanNotes = (r.SpecialNotes ?? "").Replace("\"", "\"\"");
                string cleanLoc = r.Location.Replace("\"", "\"\"");
                sb.AppendLine($"{r.Id},\"{cleanLoc}\",{r.Category},{r.EstimatedWeightKg},{r.Priority},{r.Status},\"{r.AssignedCollector}\",{r.RequestDate:yyyy-MM-dd HH:mm},\"{cleanNotes}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", $"EcoSanitize_Disposal_Logs_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        // CSV Export for Complaints
        public IActionResult ExportComplaintsCsv()
        {
            var complaints = _sanitizationService.GetAllComplaints();
            var sb = new StringBuilder();
            sb.AppendLine("Id,Title,Category,Location,WardZone,Severity,Status,ReportedBy,ReportedDate,AssignedWorker,ResolutionNotes");

            foreach (var c in complaints)
            {
                string cleanTitle = c.Title.Replace("\"", "\"\"");
                string cleanLoc = c.Location.Replace("\"", "\"\"");
                string cleanNotes = (c.ResolutionNotes ?? "").Replace("\"", "\"\"");
                sb.AppendLine($"{c.Id},\"{cleanTitle}\",{c.Category},\"{cleanLoc}\",\"{c.WardZone}\",{c.Severity},{c.Status},\"{c.ReportedBy}\",{c.ReportedDate:yyyy-MM-dd HH:mm},\"{c.AssignedWorker}\",\"{cleanNotes}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", $"EcoSanitize_Complaints_{DateTime.Now:yyyyMMdd_HHmm}.csv");
        }

        // Printable Municipal Audit Summary (PDF-ready layout)
        public IActionResult PrintableReport()
        {
            var summary = _sanitizationService.GetDashboardSummary();
            ViewBag.Requests = _sanitizationService.GetAllDisposalRequests().Take(10).ToList();
            ViewBag.Complaints = _sanitizationService.GetAllComplaints().Take(10).ToList();
            ViewBag.Audits = _sanitizationService.GetAllSanitizationAudits().Take(10).ToList();

            return View(summary);
        }
    }
}
