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

        // Sanitization Audits
        public int TotalAudits { get; set; }
        public int FollowUpAuditsCount { get; set; }
        public int CompliantAuditsCount { get; set; }

        // Complaints
        public int TotalComplaints { get; set; }
        public int PendingComplaints { get; set; }
        public int InProgressComplaints { get; set; }
        public int ResolvedComplaints { get; set; }

        // Workers & Operations
        public int ActiveWorkersCount { get; set; }
        public int TotalSchedulesCount { get; set; }

        // Recycling statistics
        public double RecyclableWeightKg { get; set; }
        public double RecyclingRatePercent { get; set; }
    }
}
