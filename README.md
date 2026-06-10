# HealthcareBillingSystem

ASP.NET Core 8 Web API built with Clean Architecture, Repository Pattern, Entity Framework Core, SQL Server, ASP.NET Identity, JWT authentication, AutoMapper, Swagger, DTOs, and dependency injection.

## Solution Structure

```text
HealthcareBillingSystem
├── Domain
│   ├── Entities
│   └── Interfaces
├── Application
│   ├── DTOs
│   ├── Exceptions
│   ├── Interfaces
│   ├── MappingProfiles
│   └── Services
├── Infrastructure
│   ├── Authentication
│   ├── Data
│   └── Repositories
├── Presentation
│   ├── Controllers
│   └── Middleware
└── API
```

## Default Admin Seed

- Email: `admin@healthcare.local`
- Password: `Admin@12345`
- Role: `Admin`

## Fix SQL Server Connection Error

If Swagger returns `No connection could be made because the target machine actively refused it`, SQL Server is not running on `localhost:1433`.

Start SQL Server with Docker:

```bash
docker compose up -d
```

Then run the API in Development mode:

```bash
dotnet run --project HealthcareBillingSystem/API/HealthcareBillingSystem.API.csproj
```

In Development, the API automatically applies EF Core migrations and seeds the `Admin` and `Doctor` roles plus the default admin user.

## Migration Commands

Run these commands from the repository root:

```bash
dotnet ef migrations add InitialCreate \
  --project HealthcareBillingSystem/Infrastructure/HealthcareBillingSystem.Infrastructure.csproj \
  --startup-project HealthcareBillingSystem/API/HealthcareBillingSystem.API.csproj \
  --output-dir Data/Migrations

dotnet ef database update \
  --project HealthcareBillingSystem/Infrastructure/HealthcareBillingSystem.Infrastructure.csproj \
  --startup-project HealthcareBillingSystem/API/HealthcareBillingSystem.API.csproj
```

## Build

```bash
dotnet restore HealthcareBillingSystem/HealthcareBillingSystem.sln
dotnet build HealthcareBillingSystem/HealthcareBillingSystem.sln
```
