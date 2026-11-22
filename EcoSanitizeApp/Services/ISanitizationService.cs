using System.Collections.Generic;
using EcoSanitizeApp.Models;

namespace EcoSanitizeApp.Services
{
    public interface ISanitizationService
    {
        IEnumerable<DisposalRequest> GetAllDisposalRequests();
        DisposalRequest? GetDisposalRequestById(int id);
        void AddDisposalRequest(DisposalRequest request);
        bool UpdateDisposalStatus(int id, string newStatus, string? collector);
        bool DeleteDisposalRequest(int id);

        IEnumerable<SanitizationAudit> GetAllSanitizationAudits();
        SanitizationAudit? GetSanitizationAuditById(int id);
        void LogSanitizationAudit(SanitizationAudit audit);

        DashboardSummary GetDashboardSummary();
    }
}
