import sqlite3
import os
from datetime import datetime

db_path = r"c:\Users\karti\Desktop\C#-project\EcoSanitize.db"

if os.path.exists(db_path):
    os.remove(db_path)

conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# 1. WasteCategories
cursor.execute("""
CREATE TABLE WasteCategories (
    CategoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    CategoryName TEXT NOT NULL UNIQUE,
    BinColorCode TEXT NOT NULL,
    HazardLevel INTEGER NOT NULL,
    RecommendedTreatment TEXT NOT NULL
);
""")

cursor.executemany("""
INSERT INTO WasteCategories (CategoryName, BinColorCode, HazardLevel, RecommendedTreatment) VALUES (?, ?, ?, ?);
""", [
    ('Organic', 'Green', 1, 'Aerobic composting and anaerobic biogas digestion'),
    ('Recyclable', 'Blue', 2, 'Mechanical sorting, shredding, and secondary material recovery'),
    ('Hazardous', 'Red', 8, 'Secure chemical encapsulation and specialized containment'),
    ('Sanitary', 'Yellow', 7, 'Segregated biohazard autoclaving and incineration'),
    ('E-Waste', 'Orange', 6, 'Precious metal recovery and certified dismantlers')
])

# 2. Users Table
cursor.execute("""
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    Username TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    Role TEXT NOT NULL,
    CreatedAt TEXT NOT NULL
);
""")

cursor.executemany("""
INSERT INTO Users (FullName, Username, Email, PasswordHash, Role, CreatedAt) VALUES (?, ?, ?, ?, ?, ?);
""", [
    ('Officer H. Sharma', 'officer', 'sharma@ecosanitize.local', 'officer123', 'Sanitation Officer', '2026-10-25 10:00:00'),
    ('Municipal Administrator', 'admin', 'admin@ecosanitize.local', 'admin123', 'System Administrator', '2026-09-25 10:00:00'),
    ('Rajesh Patel (Market Manager)', 'citizen', 'patel@market.local', 'citizen123', 'Citizen / Facility Operator', '2026-11-10 10:00:00')
])

# 3. DisposalRequests Table
cursor.execute("""
CREATE TABLE DisposalRequests (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Location TEXT NOT NULL,
    Category TEXT NOT NULL,
    EstimatedWeightKg REAL NOT NULL,
    Priority TEXT NOT NULL,
    Status TEXT NOT NULL,
    RequestDate TEXT NOT NULL,
    AssignedCollector TEXT,
    SpecialNotes TEXT
);
""")

cursor.executemany("""
INSERT INTO DisposalRequests (Location, Category, EstimatedWeightKg, Priority, Status, RequestDate, AssignedCollector, SpecialNotes) VALUES (?, ?, ?, ?, ?, ?, ?, ?);
""", [
    ('District Market Yard Gate 2', 'Organic', 120.0, 'High', 'In-Transit', '2026-11-23 09:30:00', 'Vehicle Unit 04', 'Food preparation scraps and vegetable waste'),
    ('Industrial Warehouse Complex B', 'Recyclable', 85.5, 'Medium', 'Pending', '2026-11-23 07:15:00', None, 'Flattened corrugated cartons and packaging materials'),
    ('Central Diagnostic Laboratory', 'Hazardous', 14.0, 'Emergency', 'Pending', '2026-11-23 11:00:00', 'Hazmat Unit 01', 'Spent laboratory chemical reagents'),
    ('Sub-District Health Clinic', 'Sanitary', 22.0, 'High', 'Disposed', '2026-11-22 14:00:00', 'Medical Disposal 02', 'Clinical disposables transferred for incineration'),
    ('Sector 9 Electronic Service Hub', 'Recyclable', 38.0, 'Medium', 'Pending', '2026-11-23 08:45:00', None, 'Decommissioned PC circuit boards and peripherals')
])

# 4. SanitizationAudits Table
cursor.execute("""
CREATE TABLE SanitizationAudits (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    AreaName TEXT NOT NULL,
    CleanlinessScore INTEGER NOT NULL,
    DisinfectantUsed TEXT NOT NULL,
    AuditDate TEXT NOT NULL,
    InspectorName TEXT NOT NULL,
    RequiresFollowUp INTEGER NOT NULL,
    ResultStatus TEXT NOT NULL
);
""")

cursor.executemany("""
INSERT INTO SanitizationAudits (AreaName, CleanlinessScore, DisinfectantUsed, AuditDate, InspectorName, RequiresFollowUp, ResultStatus) VALUES (?, ?, ?, ?, ?, ?, ?);
""", [
    ('Central Transit Station Public Concourse', 4, 'Sodium Hypochlorite 1% solution', '2026-11-23 06:00:00', 'Inspector H. Sharma', 0, 'Pass'),
    ('East Wholesale Market Loading Bay', 2, 'Surface Bleach Spray', '2026-11-23 07:30:00', 'Inspector R. Verma', 1, 'Needs Attention'),
    ('Community Clinic Reception Area', 5, 'Quaternary Ammonium Compound', '2026-11-22 17:00:00', 'Inspector H. Sharma', 0, 'Pass'),
    ('Municipal Ward 4 Public Restroom Block', 2, 'Standard Phenolic Detergent', '2026-11-23 10:15:00', 'Inspector R. Verma', 1, 'Needs Attention'),
    ('District Sports Complex Main Corridor', 4, 'Hydrogen Peroxide Vapor Wash', '2026-11-22 15:45:00', 'Inspector H. Sharma', 0, 'Pass')
])

# 5. Complaints Table
cursor.execute("""
CREATE TABLE Complaints (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    Category TEXT NOT NULL,
    Location TEXT NOT NULL,
    WardZone TEXT NOT NULL,
    Severity TEXT NOT NULL,
    Status TEXT NOT NULL,
    PhotoPath TEXT,
    ReportedBy TEXT NOT NULL,
    ContactNumber TEXT,
    ReportedDate TEXT NOT NULL,
    AssignedWorker TEXT,
    ResolutionNotes TEXT,
    ResolvedDate TEXT
);
""")

cursor.executemany("""
INSERT INTO Complaints (Title, Description, Category, Location, WardZone, Severity, Status, PhotoPath, ReportedBy, ContactNumber, ReportedDate, AssignedWorker, ResolutionNotes, ResolvedDate) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);
""", [
    ('Overflowing Green Bin Near Main Gate', 'Market waste spilling onto pedestrian footpath', 'Overflowing Bin', 'Sector 4 Market Main Entrance', 'Zone 1 - North Market', 'High', 'In-Progress', None, 'Sunil Gupta', '9876543210', '2026-11-23 08:00:00', 'Ramesh Kumar (Driver)', None, None),
    ('Illegal Plastic Dumping Behind Warehouse', 'Large heaps of mixed plastic sheets dumped overnight', 'Illegal Dumping', 'Behind Warehouse 12, Industrial Road', 'Zone 3 - Industrial Corridor', 'Emergency', 'Pending', None, 'Anita Desai', '9876543211', '2026-11-23 09:15:00', None, None, None),
    ('Clogged Storm Drain with Waste', 'Organic debris blocking storm water drainage channel', 'Drainage Blockage', 'Near Community Health Clinic', 'Zone 2 - Central Civic Concourse', 'Medium', 'Resolved', None, 'Dr. P. Sen', '9876543212', '2026-11-22 11:30:00', 'Suresh Yadav', 'Debris cleared, drain cleared and treated with lime powder.', '2026-11-22 16:00:00')
])

# 6. Workers Table
cursor.execute("""
CREATE TABLE Workers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    WorkerCode TEXT NOT NULL,
    FullName TEXT NOT NULL,
    PhoneNumber TEXT NOT NULL,
    AssignedZone TEXT NOT NULL,
    Specialization TEXT NOT NULL,
    Status TEXT NOT NULL,
    AssignedVehicle TEXT,
    DateJoined TEXT NOT NULL
);
""")

cursor.executemany("""
INSERT INTO Workers (WorkerCode, FullName, PhoneNumber, AssignedZone, Specialization, Status, AssignedVehicle, DateJoined) VALUES (?, ?, ?, ?, ?, ?, ?, ?);
""", [
    ('WKR-101', 'Ramesh Kumar', '9876500001', 'Zone 1 - North Market', 'Heavy Waste Compactor Driver', 'Active', 'Vehicle Unit 04', '2024-03-15'),
    ('WKR-102', 'Suresh Yadav', '9876500002', 'Zone 2 - Central Civic Concourse', 'Sanitization Crew Supervisor', 'Active', 'Sanitization Sprayer 02', '2023-08-10'),
    ('WKR-103', 'Vikram Singh', '9876500003', 'Zone 3 - Industrial Corridor', 'Hazmat Waste Specialist', 'On-Duty', 'Hazmat Van 01', '2025-01-20'),
    ('WKR-104', 'Mahesh Patil', '9876500004', 'Zone 4 - South Residential Sector', 'Door-to-Door Collection Lead', 'On-Leave', 'Mini Tipper 08', '2024-11-05')
])

# 7. CollectionSchedules Table
cursor.execute("""
CREATE TABLE CollectionSchedules (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    WardZone TEXT NOT NULL,
    CollectionDay TEXT NOT NULL,
    TimeSlot TEXT NOT NULL,
    WasteStream TEXT NOT NULL,
    AssignedVehicle TEXT NOT NULL,
    AssignedWorker TEXT NOT NULL,
    RouteDescription TEXT,
    IsActive INTEGER NOT NULL
);
""")

cursor.executemany("""
INSERT INTO CollectionSchedules (WardZone, CollectionDay, TimeSlot, WasteStream, AssignedVehicle, AssignedWorker, RouteDescription, IsActive) VALUES (?, ?, ?, ?, ?, ?, ?, ?);
""", [
    ('Zone 1 - North Market', 'Monday', '06:00 - 09:00', 'Organic (Green)', 'Compactor Unit 04', 'Ramesh Kumar', 'Wholesale Market -> Station Road -> Composting Facility', 1),
    ('Zone 1 - North Market', 'Thursday', '06:00 - 09:00', 'Recyclable (Blue)', 'Mini Tipper 08', 'Ramesh Kumar', 'Commercial Shops -> Paper Mills -> Recycler Facility', 1),
    ('Zone 2 - Central Civic Concourse', 'Wednesday', '07:00 - 10:00', 'Organic (Green)', 'Tipper Unit 02', 'Suresh Yadav', 'Bus Station -> Civic Office -> Park Concourse', 1),
    ('Zone 3 - Industrial Corridor', 'Friday', '14:00 - 17:00', 'Hazardous (Red)', 'Hazmat Van 01', 'Vikram Singh', 'Laboratories -> Chemical Plants -> Secure Encapsulation', 1)
])

conn.commit()
conn.close()

print(f"SQLite database created successfully at: {db_path}")
print("Tables created: WasteCategories, Users, DisposalRequests, SanitizationAudits, Complaints, Workers, CollectionSchedules")
