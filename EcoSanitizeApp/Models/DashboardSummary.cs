namespace EcoSanitizeApp.Models
{
    public class DashboardSummary
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InTransitRequests { get; set; }
        public int DisposedRequests { get; set; }
        public int HighPriorityRequests { get; set; }
        public double TotalWeightKg { get; set; }

        public int TotalAudits { get; set; }
        public int FollowUpAuditsCount { get; set; }
        public int CompliantAuditsCount { get; set; }
    }
}
