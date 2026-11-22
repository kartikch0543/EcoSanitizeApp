# EcoSanitize: Municipal Waste Segregation, Disposal Tracking and Sanitization Portal

A web application built with ASP.NET Core MVC and REST Web API for municipal waste management, collection dispatch tracking, and public facility sanitization auditing.

## Problem Statement Solutions

1. **Waste Segregation**: Classifies waste at source into four standardized municipal streams: Organic (Green), Recyclable (Blue), Hazardous (Red), and Sanitary (Yellow), with container specifications, target facility routing, and hazard ratings.
2. **Disposal Tracking**: Queue management for waste pickup requests, urgency triage (Emergency, High, Medium, Low), vehicle dispatch assignment, and status lifecycle tracking from pending collection through completed disposal.
3. **Sanitization Management**: Structured hygiene inspections for public spaces and municipal facilities with 1 to 5 cleanliness ratings, chemical disinfectant verification (Sodium Hypochlorite, Quaternary Ammonium compounds), and automated action flags for required deep cleaning.

## Project Structure

- `Controllers/`:
  - `HomeController.cs`: Dashboard overview, segregation guidelines, Privacy Policy, Terms and Conditions.
  - `DisposalController.cs`: Waste collection request logging, status updates, and stream details.
  - `SanitizationController.cs`: Facility hygiene inspection logging and audit reports.
  - `Api/WasteApiController.cs`: REST Web API endpoints for collection requests and sanitization audits.
- `Models/`: Entity models for disposal requests, sanitization audits, segregation streams, and dashboard summaries.
- `Services/`: `ISanitizationService` and `SanitizationService` managing business logic and data access.
- `Data/`: `ApplicationDbContext` with EF Core and pre-seeded municipal records.
- `Views/`: Server-rendered Razor views styled with a utility theme.
- `Database/`: SQL Server table definitions, constraints, joins, views, and stored procedures.

## Running the Application

1. Open PowerShell or a terminal in the project directory:
   ```powershell
   cd c:\Users\karti\Desktop\C#-project\EcoSanitizeApp
   dotnet restore
   dotnet build
   dotnet run
   ```
2. Open your browser and navigate to:
   - Web Portal: `http://localhost:5197`
   - REST API: `http://localhost:5197/api/wasteapi`
