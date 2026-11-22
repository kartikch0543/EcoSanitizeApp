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

        public DashboardSummary GetDashboardSummary()
        {
            var requests = _context.DisposalRequests.ToList();
            var audits = _context.SanitizationAudits.ToList();

            return new DashboardSummary
            {
                TotalRequests = requests.Count,
                PendingRequests = requests.Count(r => r.Status == "Pending"),
                InTransitRequests = requests.Count(r => r.Status == "In-Transit"),
                DisposedRequests = requests.Count(r => r.Status == "Disposed"),
                HighPriorityRequests = requests.Count(r => r.Priority == "High" || r.Priority == "Emergency"),
                TotalWeightKg = Math.Round(requests.Sum(r => r.EstimatedWeightKg), 1),
                TotalAudits = audits.Count,
                FollowUpAuditsCount = audits.Count(a => a.RequiresFollowUp),
                CompliantAuditsCount = audits.Count(a => !a.RequiresFollowUp)
            };
        }
    }
}
