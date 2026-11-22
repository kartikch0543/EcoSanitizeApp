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
        }
    }
}
