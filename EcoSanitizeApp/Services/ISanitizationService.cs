using System.Collections.Generic;
using EcoSanitizeApp.Models;

namespace EcoSanitizeApp.Services
{
    public interface ISanitizationService
    {
        // Disposal Requests
        IEnumerable<DisposalRequest> GetAllDisposalRequests();
        DisposalRequest? GetDisposalRequestById(int id);
        void AddDisposalRequest(DisposalRequest request);
        bool UpdateDisposalStatus(int id, string newStatus, string? collector);
        bool DeleteDisposalRequest(int id);

        // Sanitization Audits
        IEnumerable<SanitizationAudit> GetAllSanitizationAudits();
        SanitizationAudit? GetSanitizationAuditById(int id);
        void LogSanitizationAudit(SanitizationAudit audit);

        // Complaints Management
        IEnumerable<SanitationComplaint> GetAllComplaints();
        SanitationComplaint? GetComplaintById(int id);
        void AddComplaint(SanitationComplaint complaint);
        bool UpdateComplaintStatus(int id, string status, string? assignedWorker, string? resolutionNotes);

        // Sanitation Workers
        IEnumerable<SanitationWorker> GetAllWorkers();
        SanitationWorker? GetWorkerById(int id);
        void AddWorker(SanitationWorker worker);
        bool UpdateWorkerStatus(int id, string status, string? zone);

        // Collection Schedules
        IEnumerable<CollectionSchedule> GetAllSchedules();
        CollectionSchedule? GetScheduleById(int id);
        void AddSchedule(CollectionSchedule schedule);
        bool ToggleScheduleStatus(int id);

        // Summary and Analytics
        DashboardSummary GetDashboardSummary();
    }
}
