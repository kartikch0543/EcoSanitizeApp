using System;
using Microsoft.EntityFrameworkCore;
using EcoSanitizeApp.Models;

namespace EcoSanitizeApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DisposalRequest> DisposalRequests { get; set; }
        public DbSet<SanitizationAudit> SanitizationAudits { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<SanitationComplaint> Complaints { get; set; }
        public DbSet<SanitationWorker> Workers { get; set; }
        public DbSet<CollectionSchedule> CollectionSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial users for login
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    FullName = "Officer H. Sharma",
                    Username = "officer",
                    Email = "sharma@ecosanitize.local",
                    PasswordHash = "officer123",
                    Role = "Sanitation Officer",
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new AppUser
                {
                    Id = 2,
                    FullName = "Municipal Administrator",
                    Username = "admin",
                    Email = "admin@ecosanitize.local",
                    PasswordHash = "admin123",
                    Role = "System Administrator",
                    CreatedAt = DateTime.Now.AddDays(-60)
                },
                new AppUser
                {
                    Id = 3,
                    FullName = "Rajesh Patel (Market Manager)",
                    Username = "citizen",
                    Email = "patel@market.local",
                    PasswordHash = "citizen123",
                    Role = "Citizen / Facility Operator",
                    CreatedAt = DateTime.Now.AddDays(-15)
                }
            );

            // Seed initial disposal requests
            modelBuilder.Entity<DisposalRequest>().HasData(
                new DisposalRequest
                {
                    Id = 1,
                    Location = "District Market Yard Gate 2",
                    Category = "Organic",
                    EstimatedWeightKg = 120.0,
                    Priority = "High",
                    Status = "In-Transit",
                    RequestDate = DateTime.Now.AddHours(-3),
                    AssignedCollector = "Vehicle Unit 04",
                    SpecialNotes = "Food preparation scraps and vegetable waste from morning market."
                },
                new DisposalRequest
                {
                    Id = 2,
                    Location = "Industrial Warehouse Complex B",
                    Category = "Recyclable",
                    EstimatedWeightKg = 85.5,
                    Priority = "Medium",
                    Status = "Pending",
                    RequestDate = DateTime.Now.AddHours(-2),
                    AssignedCollector = null,
                    SpecialNotes = "Flattened corrugated cartons and packaging materials."
                },
                new DisposalRequest
                {
                    Id = 3,
                    Location = "Central Diagnostic Laboratory",
                    Category = "Hazardous",
                    EstimatedWeightKg = 14.0,
                    Priority = "Emergency",
                    Status = "Pending",
                    RequestDate = DateTime.Now.AddHours(-1),
                    AssignedCollector = "Hazmat Unit 01",
                    SpecialNotes = "Spent laboratory chemical reagents. Chemical safety gear required."
                },
                new DisposalRequest
                {
                    Id = 4,
                    Location = "Sub-District Health Clinic",
                    Category = "Sanitary",
                    EstimatedWeightKg = 22.0,
                    Priority = "High",
                    Status = "Disposed",
                    RequestDate = DateTime.Now.AddDays(-1),
                    AssignedCollector = "Medical Disposal 02",
                    SpecialNotes = "Segregated clinical disposables transferred for incineration."
                },
                new DisposalRequest
                {
                    Id = 5,
                    Location = "City IT Tech Park Tower 3",
                    Category = "E-Waste",
                    EstimatedWeightKg = 45.0,
                    Priority = "Medium",
                    Status = "Pending",
                    RequestDate = DateTime.Now.AddHours(-5),
                    AssignedCollector = "E-Waste Mobile Unit 01",
                    SpecialNotes = "Decommissioned desktop units, motherboards, and battery power banks."
                }
            );

            // Seed initial sanitization audits
            modelBuilder.Entity<SanitizationAudit>().HasData(
                new SanitizationAudit
                {
                    Id = 1,
                    AreaName = "Central Transit Station Public Concourse",
                    CleanlinessScore = 4,
                    DisinfectantUsed = "Sodium Hypochlorite 1% solution",
                    AuditDate = DateTime.Now.AddHours(-5),
                    InspectorName = "Inspector H. Sharma",
                    RequiresFollowUp = false,
                    ResultStatus = "Pass"
                },
                new SanitizationAudit
                {
                    Id = 2,
                    AreaName = "East Wholesale Market Loading Bay",
                    CleanlinessScore = 2,
                    DisinfectantUsed = "Surface Bleach Spray",
                    AuditDate = DateTime.Now.AddHours(-2),
                    InspectorName = "Inspector R. Verma",
                    RequiresFollowUp = true,
                    ResultStatus = "Needs Attention"
                },
                new SanitizationAudit
                {
                    Id = 3,
                    AreaName = "Community Clinic Reception Area",
                    CleanlinessScore = 5,
                    DisinfectantUsed = "Quaternary Ammonium Compound",
                    AuditDate = DateTime.Now.AddHours(-1),
                    InspectorName = "Inspector H. Sharma",
                    RequiresFollowUp = false,
                    ResultStatus = "Pass"
                }
            );

            // Seed Sanitation Workers
            modelBuilder.Entity<SanitationWorker>().HasData(
                new SanitationWorker
                {
                    Id = 1,
                    WorkerCode = "WRK-101",
                    FullName = "Amit Sharma",
                    PhoneNumber = "+91 98765 43210",
                    AssignedZone = "Zone 1 - North Market",
                    Specialization = "Waste Collection Driver",
                    Status = "Active",
                    AssignedVehicle = "Compactor Truck 02",
                    DateJoined = DateTime.Now.AddMonths(-18)
                },
                new SanitationWorker
                {
                    Id = 2,
                    WorkerCode = "WRK-102",
                    FullName = "Rajesh Varma",
                    PhoneNumber = "+91 98765 43211",
                    AssignedZone = "Zone 2 - Central Commercial",
                    Specialization = "Biohazard Handler",
                    Status = "On-Duty",
                    AssignedVehicle = "Hazmat Unit 01",
                    DateJoined = DateTime.Now.AddMonths(-12)
                },
                new SanitationWorker
                {
                    Id = 3,
                    WorkerCode = "WRK-103",
                    FullName = "Sunita Devi",
                    PhoneNumber = "+91 98765 43212",
                    AssignedZone = "Zone 3 - West Industrial",
                    Specialization = "Drainage & Sewer Cleaner",
                    Status = "Active",
                    AssignedVehicle = "Mini Tipper 05",
                    DateJoined = DateTime.Now.AddMonths(-24)
                },
                new SanitationWorker
                {
                    Id = 4,
                    WorkerCode = "WRK-104",
                    FullName = "Manoj Kumar",
                    PhoneNumber = "+91 98765 43213",
                    AssignedZone = "Zone 4 - South Residential",
                    Specialization = "Disinfection Inspector",
                    Status = "Active",
                    AssignedVehicle = "Sprayer Van 03",
                    DateJoined = DateTime.Now.AddMonths(-8)
                }
            );

            // Seed Sanitation Complaints
            modelBuilder.Entity<SanitationComplaint>().HasData(
                new SanitationComplaint
                {
                    Id = 1,
                    Title = "Overflowing commercial garbage bins near Gate 3",
                    Description = "Public bins are overflowing since morning and spilling onto pedestrian walkway. Strong odor causing inconvenience.",
                    Category = "Overflowing Bin",
                    Location = "Sector 4 Commercial Plaza",
                    WardZone = "Zone 2 - Central Commercial",
                    Severity = "High",
                    Status = "Pending",
                    PhotoPath = null,
                    ReportedBy = "Vikram Malhotra",
                    ContactNumber = "+91 98111 22334",
                    ReportedDate = DateTime.Now.AddHours(-4),
                    AssignedWorker = null,
                    ResolutionNotes = null,
                    ResolvedDate = null
                },
                new SanitationComplaint
                {
                    Id = 2,
                    Title = "Blocked drainage channel causing wastewater stagnant pool",
                    Description = "Drainage grill choked with plastic bags and silt after yesterday rain. Mosquito breeding hazard near health center.",
                    Category = "Blocked Drainage",
                    Location = "Near Community Health Center, Link Road",
                    WardZone = "Zone 1 - North Market",
                    Severity = "Critical",
                    Status = "In-Progress",
                    PhotoPath = null,
                    ReportedBy = "Dr. Ananya Sen",
                    ContactNumber = "+91 98222 33445",
                    ReportedDate = DateTime.Now.AddHours(-18),
                    AssignedWorker = "Sunita Devi",
                    ResolutionNotes = "Silt clearance team deployed with vacuum tanker.",
                    ResolvedDate = null
                },
                new SanitationComplaint
                {
                    Id = 3,
                    Title = "Illegal construction debris and broken drywall dumped on vacant lot",
                    Description = "Approx 2 truckloads of concrete debris, broken gypsum, and packaging dumped overnight blocking emergency lane.",
                    Category = "Illegal Dumping",
                    Location = "Plot 42, Industrial Sector B",
                    WardZone = "Zone 3 - West Industrial",
                    Severity = "Medium",
                    Status = "Resolved",
                    PhotoPath = null,
                    ReportedBy = "Suresh Gupta",
                    ContactNumber = "+91 98333 44556",
                    ReportedDate = DateTime.Now.AddDays(-2),
                    AssignedWorker = "Amit Sharma",
                    ResolutionNotes = "Debris cleared and transferred to construction recycling depot.",
                    ResolvedDate = DateTime.Now.AddHours(-6)
                },
                new SanitationComplaint
                {
                    Id = 4,
                    Title = "Foul odor and unwashed floor at municipal public convenience",
                    Description = "Sanitization required. Exhaust fan non-functional and floor needs high-pressure disinfectant wash.",
                    Category = "Public Restroom",
                    Location = "Main Bus Terminal, West Concourse",
                    WardZone = "Zone 2 - Central Commercial",
                    Severity = "High",
                    Status = "Pending",
                    PhotoPath = null,
                    ReportedBy = "Priya Nair",
                    ContactNumber = "+91 98444 55667",
                    ReportedDate = DateTime.Now.AddHours(-2),
                    AssignedWorker = null,
                    ResolutionNotes = null,
                    ResolvedDate = null
                }
            );

            // Seed Collection Schedules
            modelBuilder.Entity<CollectionSchedule>().HasData(
                new CollectionSchedule
                {
                    Id = 1,
                    WardZone = "Zone 1 - North Market",
                    CollectionDay = "Monday",
                    TimeSlot = "06:00 - 09:00",
                    WasteStream = "Organic",
                    AssignedVehicle = "Compactor Truck 01",
                    AssignedWorker = "Amit Sharma",
                    RouteDescription = "Wholesale vegetable market and dining strip morning sweep",
                    IsActive = true
                },
                new CollectionSchedule
                {
                    Id = 2,
                    WardZone = "Zone 1 - North Market",
                    CollectionDay = "Wednesday",
                    TimeSlot = "09:00 - 12:00",
                    WasteStream = "Recyclable",
                    AssignedVehicle = "Dry Waste Van 04",
                    AssignedWorker = "Amit Sharma",
                    RouteDescription = "Commercial packaging, cardboard carton pickup",
                    IsActive = true
                },
                new CollectionSchedule
                {
                    Id = 3,
                    WardZone = "Zone 2 - Central Commercial",
                    CollectionDay = "Daily",
                    TimeSlot = "06:00 - 09:00",
                    WasteStream = "Organic",
                    AssignedVehicle = "Compactor Truck 02",
                    AssignedWorker = "Rajesh Varma",
                    RouteDescription = "Office towers and shopping concourse collection",
                    IsActive = true
                },
                new CollectionSchedule
                {
                    Id = 4,
                    WardZone = "Zone 2 - Central Commercial",
                    CollectionDay = "Tuesday",
                    TimeSlot = "14:00 - 17:00",
                    WasteStream = "E-Waste",
                    AssignedVehicle = "E-Waste Mobile Unit 01",
                    AssignedWorker = "Rajesh Varma",
                    RouteDescription = "Electronics shops and IT office hardware collection",
                    IsActive = true
                },
                new CollectionSchedule
                {
                    Id = 5,
                    WardZone = "Zone 3 - West Industrial",
                    CollectionDay = "Thursday",
                    TimeSlot = "08:00 - 11:00",
                    WasteStream = "Hazardous",
                    AssignedVehicle = "Hazmat Unit 01",
                    AssignedWorker = "Sunita Devi",
                    RouteDescription = "Chemical workshops and battery retail cluster",
                    IsActive = true
                },
                new CollectionSchedule
                {
                    Id = 6,
                    WardZone = "Zone 4 - South Residential",
                    CollectionDay = "Friday",
                    TimeSlot = "07:00 - 10:00",
                    WasteStream = "Sanitary",
                    AssignedVehicle = "Medical Van 02",
                    AssignedWorker = "Manoj Kumar",
                    RouteDescription = "Private clinics and residential nursing home route",
                    IsActive = true
                }
            );
        }
    }
}
