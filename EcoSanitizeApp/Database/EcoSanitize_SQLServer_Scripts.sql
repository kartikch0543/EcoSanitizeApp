IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EcoSanitizeDB')
BEGIN
    CREATE DATABASE EcoSanitizeDB;
END
GO

USE EcoSanitizeDB;
GO

IF OBJECT_ID('dbo.WasteCategories', 'U') IS NOT NULL DROP TABLE dbo.WasteCategories;
CREATE TABLE dbo.WasteCategories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL UNIQUE,
    BinColorCode NVARCHAR(30) NOT NULL,
    HazardLevel INT NOT NULL CHECK (HazardLevel BETWEEN 1 AND 10),
    RecommendedTreatment NVARCHAR(150) NOT NULL
);
GO

IF OBJECT_ID('dbo.DisposalRequests', 'U') IS NOT NULL DROP TABLE dbo.DisposalRequests;
CREATE TABLE dbo.DisposalRequests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Location NVARCHAR(100) NOT NULL,
    CategoryId INT NOT NULL,
    EstimatedWeightKg DECIMAL(6,2) NOT NULL CHECK (EstimatedWeightKg > 0.0),
    Priority NVARCHAR(20) NOT NULL CHECK (Priority IN ('Low', 'Medium', 'High', 'Emergency')),
    Status NVARCHAR(30) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Scheduled', 'In-Transit', 'Collected', 'Disposed')),
    RequestDate DATETIME NOT NULL DEFAULT GETDATE(),
    AssignedCollector NVARCHAR(80) NULL,
    SpecialNotes NVARCHAR(250) NULL,
    CONSTRAINT FK_DisposalRequests_WasteCategories FOREIGN KEY (CategoryId) 
        REFERENCES dbo.WasteCategories(CategoryId) ON DELETE CASCADE
);
GO

IF OBJECT_ID('dbo.SanitizationAudits', 'U') IS NOT NULL DROP TABLE dbo.SanitizationAudits;
CREATE TABLE dbo.SanitizationAudits (
    AuditId INT IDENTITY(1,1) PRIMARY KEY,
    AreaName NVARCHAR(100) NOT NULL,
    CleanlinessScore INT NOT NULL CHECK (CleanlinessScore BETWEEN 1 AND 5),
    DisinfectantUsed NVARCHAR(100) NOT NULL,
    AuditDate DATETIME NOT NULL DEFAULT GETDATE(),
    InspectorName NVARCHAR(80) NOT NULL,
    RequiresFollowUp BIT NOT NULL DEFAULT 0,
    ResultStatus NVARCHAR(30) NOT NULL DEFAULT 'Pass' CHECK (ResultStatus IN ('Pass', 'Needs Attention', 'Critical'))
);
GO

INSERT INTO dbo.WasteCategories (CategoryName, BinColorCode, HazardLevel, RecommendedTreatment) VALUES
('Organic', 'Green', 1, 'Aerobic composting and anaerobic biogas digestion'),
('Recyclable', 'Blue', 2, 'Mechanical sorting, shredding, and secondary material recovery'),
('Hazardous', 'Red', 8, 'Secure chemical encapsulation and specialized hazardous containment'),
('Sanitary', 'Yellow', 7, 'Segregated biohazard autoclaving and high-temperature incineration');

INSERT INTO dbo.DisposalRequests (Location, CategoryId, EstimatedWeightKg, Priority, Status, AssignedCollector, SpecialNotes) VALUES
('District Market Yard Gate 2', 1, 120.00, 'High', 'In-Transit', 'Vehicle Unit 04', 'Food preparation scraps and vegetable waste'),
('Industrial Warehouse Complex B', 2, 85.50, 'Medium', 'Pending', NULL, 'Flattened corrugated cartons and packaging materials'),
('Central Diagnostic Laboratory', 3, 14.00, 'Emergency', 'Pending', 'Hazmat Unit 01', 'Spent laboratory chemical reagents'),
('Sub-District Health Clinic', 4, 22.00, 'High', 'Disposed', 'Medical Disposal 02', 'Clinical disposables transferred for incineration');

INSERT INTO dbo.SanitizationAudits (AreaName, CleanlinessScore, DisinfectantUsed, InspectorName, RequiresFollowUp, ResultStatus) VALUES
('Central Transit Station Public Concourse', 4, 'Sodium Hypochlorite 1% solution', 'Inspector H. Sharma', 0, 'Pass'),
('East Wholesale Market Loading Bay', 2, 'Surface Bleach Spray', 'Inspector R. Verma', 1, 'Needs Attention'),
('Community Clinic Reception Area', 5, 'Quaternary Ammonium Compound', 'Inspector H. Sharma', 0, 'Pass');
GO

SELECT 
    d.Id AS RequestId,
    d.Location,
    c.CategoryName,
    c.BinColorCode,
    d.EstimatedWeightKg,
    d.Priority,
    d.Status,
    d.AssignedCollector
FROM dbo.DisposalRequests d
INNER JOIN dbo.WasteCategories c ON d.CategoryId = c.CategoryId
WHERE d.Status <> 'Disposed' AND d.EstimatedWeightKg > 10.0
ORDER BY d.EstimatedWeightKg DESC;
GO

SELECT 
    c.CategoryName,
    COUNT(d.Id) AS TotalDisposalJobs,
    SUM(d.EstimatedWeightKg) AS TotalWeightKg,
    AVG(d.EstimatedWeightKg) AS AvgWeightPerJob
FROM dbo.DisposalRequests d
INNER JOIN dbo.WasteCategories c ON d.CategoryId = c.CategoryId
GROUP BY c.CategoryName
HAVING SUM(d.EstimatedWeightKg) > 20.0;
GO

IF OBJECT_ID('dbo.vw_ActiveHighPriorityDisposals', 'V') IS NOT NULL DROP VIEW dbo.vw_ActiveHighPriorityDisposals;
GO
CREATE VIEW dbo.vw_ActiveHighPriorityDisposals
AS
SELECT 
    d.Id,
    d.Location,
    c.CategoryName,
    c.BinColorCode,
    d.EstimatedWeightKg,
    d.Priority,
    d.Status,
    d.RequestDate
FROM dbo.DisposalRequests d
INNER JOIN dbo.WasteCategories c ON d.CategoryId = c.CategoryId
WHERE d.Priority IN ('High', 'Emergency') AND d.Status IN ('Pending', 'Scheduled', 'In-Transit');
GO

IF OBJECT_ID('dbo.vw_SanitizationInspectionSummary', 'V') IS NOT NULL DROP VIEW dbo.vw_SanitizationInspectionSummary;
GO
CREATE VIEW dbo.vw_SanitizationInspectionSummary
AS
SELECT 
    AreaName,
    CleanlinessScore,
    DisinfectantUsed,
    InspectorName,
    ResultStatus,
    AuditDate
FROM dbo.SanitizationAudits
WHERE RequiresFollowUp = 1;
GO

IF OBJECT_ID('dbo.sp_GetDisposalsByPriority', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetDisposalsByPriority;
GO
CREATE PROCEDURE dbo.sp_GetDisposalsByPriority
    @PriorityLevel NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        d.Id,
        d.Location,
        c.CategoryName,
        d.EstimatedWeightKg,
        d.Priority,
        d.Status,
        d.AssignedCollector
    FROM dbo.DisposalRequests d
    INNER JOIN dbo.WasteCategories c ON d.CategoryId = c.CategoryId
    WHERE d.Priority = @PriorityLevel
    ORDER BY d.RequestDate DESC;
END
GO

IF OBJECT_ID('dbo.sp_LogSanitizationAudit', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_LogSanitizationAudit;
GO
CREATE PROCEDURE dbo.sp_LogSanitizationAudit
    @AreaName NVARCHAR(100),
    @CleanlinessScore INT,
    @DisinfectantUsed NVARCHAR(100),
    @InspectorName NVARCHAR(80)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @FollowUp BIT = 0;
    DECLARE @Status NVARCHAR(30) = 'Pass';

    IF @CleanlinessScore <= 2
    BEGIN
        SET @FollowUp = 1;
        SET @Status = 'Needs Attention';
    END

    INSERT INTO dbo.SanitizationAudits 
        (AreaName, CleanlinessScore, DisinfectantUsed, InspectorName, RequiresFollowUp, ResultStatus, AuditDate)
    VALUES 
        (@AreaName, @CleanlinessScore, @DisinfectantUsed, @InspectorName, @FollowUp, @Status, GETDATE());

    SELECT SCOPE_IDENTITY() AS NewAuditId;
END
GO