# Employee Management System API

REST API mini project untuk portofolio — dibangun dengan **ASP.NET Core 8 Web API** dan **PostgreSQL** menggunakan Entity Framework Core.

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core 8 Web API |
| Database | PostgreSQL |
| ORM | Entity Framework Core 8 |
| Provider | Npgsql |
| Documentation | Swagger / OpenAPI |

## Features

- CRUD **Departments** (Human Resources, Engineering, Finance, dll.)
- CRUD **Employees** dengan relasi ke Department
- Filter employees by `departmentId` dan `isActive`
- Validasi input dengan Data Annotations
- Seed data awal (3 departments, 3 employees)
- Auto database migration saat startup
- Swagger UI untuk testing API

## Project Structure

```
EmployeeManagementSystem/
├── EmployeeManagementSystem.sln
└── EmployeeManagementSystem.Api/
    ├── Controllers/       # API endpoints
    ├── Data/              # DbContext & migrations
    ├── DTOs/              # Request/Response models
    ├── Models/            # Entity models
    ├── Services/          # Business logic
    └── Program.cs
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (v14+ recommended)
- EF Core CLI tools (optional, for manual migrations):

```bash
dotnet tool install --global dotnet-ef
```

## Database Setup

1. Install dan jalankan PostgreSQL
2. Buat database (opsional — migration akan membuat schema otomatis):

```sql
CREATE DATABASE "EmployeeManagementDb";
```

3. Update connection string di `appsettings.json` atau `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=EmployeeManagementDb;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

## Run the Application

```bash
cd EmployeeManagementSystem.Api
dotnet restore
dotnet run
```

API akan berjalan di:
- HTTP: `http://localhost:5175`
- HTTPS: `https://localhost:7120`
- Swagger UI: `http://localhost:5175/swagger`

## API Endpoints

### Departments

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/departments` | Get all departments |
| GET | `/api/departments/{id}` | Get department by ID |
| POST | `/api/departments` | Create department |
| PUT | `/api/departments/{id}` | Update department |
| DELETE | `/api/departments/{id}` | Delete department |

### Employees

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employees` | Get all employees |
| GET | `/api/employees?departmentId=2` | Filter by department |
| GET | `/api/employees?isActive=true` | Filter by active status |
| GET | `/api/employees/{id}` | Get employee by ID |
| POST | `/api/employees` | Create employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee |

## Example Request

**Create Employee:**

```json
POST /api/employees
Content-Type: application/json

{
  "firstName": "Alice",
  "lastName": "Brown",
  "email": "alice.brown@company.com",
  "phone": "+62-812-1111-2222",
  "hireDate": "2024-06-01",
  "salary": 7800000,
  "isActive": true,
  "departmentId": 2
}
```

## Migrations (Manual)

Jika ingin menjalankan migration secara manual:

```bash
cd EmployeeManagementSystem.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Docker PostgreSQL (Optional)

Jalankan PostgreSQL dengan Docker:

```bash
docker run --name ems-postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=EmployeeManagementDb -p 5432:5432 -d postgres:16
```

## License

MIT — Free to use for portfolio and learning purposes.
