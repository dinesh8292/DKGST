# DKGST Setup Guide

## Prerequisites
- .NET 8 SDK
- PostgreSQL 14+ or SQL Server 2019+
- Docker & Docker Compose (optional)

## Database Configuration

### PostgreSQL
```bash
createdb dkgst_master
createdb dkgst_company_001
```

### SQL Server
```sql
CREATE DATABASE DKGST_Master;
CREATE DATABASE DKGST_Company_001;
```

## Environment Setup

Create `appsettings.json` in `src/DKGST.API/`:

```json
{
  "ConnectionStrings": {
    "MasterDb": "Host=localhost;Database=dkgst_master;Username=postgres;Password=password",
    "CompanyDb": "Host=localhost;Database=dkgst_company_{0};Username=postgres;Password=password"
  },
  "DatabaseProvider": "PostgreSQL",
  "JwtSettings": {
    "Secret": "your-secret-key-here-min-32-characters-long",
    "ExpiryMinutes": 60
  },
  "Languages": ["en", "es", "hi"]
}
```

## Build & Run

```bash
# Build
dotnet build

# Apply Migrations
cd src/DKGST.Infrastructure
dotnet ef database update

# Run API
cd ../DKGST.API
dotnet run

# Run Blazor (new terminal)
cd src/DKGST.Blazor
dotnet run
```

## Access
- API: https://localhost:5001
- Swagger: https://localhost:5001/swagger
- Blazor: https://localhost:7001