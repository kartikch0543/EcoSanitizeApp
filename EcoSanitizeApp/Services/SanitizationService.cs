using System;
using System.Collections.Generic;
using System.Linq;
using EcoSanitizeApp.Data;
using EcoSanitizeApp.Models;

namespace EcoSanitizeApp.Services
{
    public class SanitizationService : ISanitizationService
    {
        private readonly ApplicationDbContext _context;

        public SanitizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Disposal Requests
        public IEnumerable<DisposalRequest> GetAllDisposalRequests()
        {
            return _context.DisposalRequests.OrderByDescending(r => r.RequestDate).ToList();
        }

        public DisposalRequest? GetDisposalRequestById(int id)
        {
            return _context.DisposalRequests.FirstOrDefault(r => r.Id == id);
        }

        public void AddDisposalRequest(DisposalRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Location))
            {
                throw new ArgumentException("Collection location cannot be empty.");
            }

            _context.DisposalRequests.Add(request);
            _context.SaveChanges();
        }

        public bool UpdateDisposalStatus(int id, string newStatus, string? collector)
        {
            var item = _context.DisposalRequests.Find(id);
            if (item == null) return false;

            item.Status = newStatus;
            if (!string.IsNullOrWhiteSpace(collector))
            {
                item.AssignedCollector = collector;
            }

            _context.SaveChanges();
            return true;
        }

        public bool DeleteDisposalRequest(int id)
        {
            var item = _context.DisposalRequests.Find(id);
            if (item == null) return false;

            _context.DisposalRequests.Remove(item);
            _context.SaveChanges();
            return true;
        }

        // Sanitization Audits
        public IEnumerable<SanitizationAudit> GetAllSanitizationAudits()
        {
            return _context.SanitizationAudits.OrderByDescending(a => a.AuditDate).ToList();
        }

        public SanitizationAudit? GetSanitizationAuditById(int id)
        {
            return _context.SanitizationAudits.FirstOrDefault(a => a.Id == id);
        }

        public void LogSanitizationAudit(SanitizationAudit audit)
        {
            if (audit.CleanlinessScore <= 2)
            {
                audit.RequiresFollowUp = true;
                audit.ResultStatus = "Needs Attention";
            }
            else
            {
                audit.RequiresFollowUp = false;
                audit.ResultStatus = "Pass";
            }

            _context.SanitizationAudits.Add(audit);
            _context.SaveChanges();
        }

        // Complaints Management
        public IEnumerable<SanitationComplaint> GetAllComplaints()
        {
            return _context.Complaints.OrderByDescending(c => c.ReportedDate).ToList();
        }

        public SanitationComplaint? GetComplaintById(int id)
        {
            return _context.Complaints.FirstOrDefault(c => c.Id == id);
        }

        public void AddComplaint(SanitationComplaint complaint)
        {
            if (string.IsNullOrWhiteSpace(complaint.Title) || string.IsNullOrWhiteSpace(complaint.Location))
            {
                throw new ArgumentException("Complaint title and location are required.");
            }

            _context.Complaints.Add(complaint);
            _context.SaveChanges();
        }

        public bool UpdateComplaintStatus(int id, string status, string? assignedWorker, string? resolutionNotes)
        {
            var item = _context.Complaints.Find(id);
            if (item == null) return false;

            item.Status = status;
            if (!string.IsNullOrWhiteSpace(assignedWorker))
            {
                item.AssignedWorker = assignedWorker;
            }

            if (!string.IsNullOrWhiteSpace(resolutionNotes))
            {
                item.ResolutionNotes = resolutionNotes;
            }

            if (status == "Resolved" && item.ResolvedDate == null)
            {
                item.ResolvedDate = DateTime.Now;
            }

            _context.SaveChanges();
            return true;
        }

        // Sanitation Workers
        public IEnumerable<SanitationWorker> GetAllWorkers()
        {
            return _context.Workers.OrderBy(w => w.FullName).ToList();
        }

        public SanitationWorker? GetWorkerById(int id)
        {
            return _context.Workers.FirstOrDefault(w => w.Id == id);
        }

        public void AddWorker(SanitationWorker worker)
        {
            _context.Workers.Add(worker);
            _context.SaveChanges();
        }

        public bool UpdateWorkerStatus(int id, string status, string? zone)
        {
            var item = _context.Workers.Find(id);
            if (item == null) return false;

            item.Status = status;
            if (!string.IsNullOrWhiteSpace(zone))
            {
                item.AssignedZone = zone;
            }

            _context.SaveChanges();
            return true;
        }

        // Collection Schedules
        public IEnumerable<CollectionSchedule> GetAllSchedules()
        {
            return _context.CollectionSchedules.OrderBy(s => s.WardZone).ThenBy(s => s.CollectionDay).ToList();
        }

        public CollectionSchedule? GetScheduleById(int id)
        {
            return _context.CollectionSchedules.FirstOrDefault(s => s.Id == id);
        }

        public void AddSchedule(CollectionSchedule schedule)
        {
            _context.CollectionSchedules.Add(schedule);
            _context.SaveChanges();
        }

        public bool ToggleScheduleStatus(int id)
        {
            var item = _context.CollectionSchedules.Find(id);
            if (item == null) return false;

            item.IsActive = !item.IsActive;
            _context.SaveChanges();
            return true;
        }

        // Dashboard and Analytics
        public DashboardSummary GetDashboardSummary()
        {
            var requests = _context.DisposalRequests.ToList();
            var audits = _context.SanitizationAudits.ToList();
            var complaints = _context.Complaints.ToList();
            var workers = _context.Workers.ToList();
            var schedules = _context.CollectionSchedules.ToList();

            double totalWeight = Math.Round(requests.Sum(r => r.EstimatedWeightKg), 1);
            double recyclableWeight = Math.Round(requests.Where(r => r.Category == "Recyclable" || r.Category == "Organic").Sum(r => r.EstimatedWeightKg), 1);
            double recyclingRate = totalWeight > 0 ? Math.Round((recyclableWeight / totalWeight) * 100.0, 1) : 0.0;

            return new DashboardSummary
            {
                TotalRequests = requests.Count,
                PendingRequests = requests.Count(r => r.Status == "Pending"),
                InTransitRequests = requests.Count(r => r.Status == "In-Transit"),
                DisposedRequests = requests.Count(r => r.Status == "Disposed"),
                HighPriorityRequests = requests.Count(r => r.Priority == "High" || r.Priority == "Emergency"),
                TotalWeightKg = totalWeight,

                TotalAudits = audits.Count,
                FollowUpAuditsCount = audits.Count(a => a.RequiresFollowUp),
                CompliantAuditsCount = audits.Count(a => !a.RequiresFollowUp),

                TotalComplaints = complaints.Count,
                PendingComplaints = complaints.Count(c => c.Status == "Pending"),
                InProgressComplaints = complaints.Count(c => c.Status == "In-Progress"),
                ResolvedComplaints = complaints.Count(c => c.Status == "Resolved"),

                ActiveWorkersCount = workers.Count(w => w.Status == "Active" || w.Status == "On-Duty"),
                TotalSchedulesCount = schedules.Count(s => s.IsActive),

                RecyclableWeightKg = recyclableWeight,
                RecyclingRatePercent = recyclingRate
            };
        }
    }
}
