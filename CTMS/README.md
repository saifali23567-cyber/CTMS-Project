# Courier Tracking Management System (CTMS)

A role-based **Courier Tracking Management System** built with **ASP.NET Core 8 Web API**, **Entity Framework Core**, **Microsoft SQL Server**, and a lightweight HTML/CSS/JavaScript frontend.

The project demonstrates REST API development, JWT authentication, role-based authorization, database integration, package management, courier assignment, and shipment tracking.

> **Project status:** Educational / portfolio project. The repository contains the complete source code and frontend, but it is configured primarily for local development rather than production deployment.

---

## Features

### Authentication & Authorization

- User registration and login
- JWT-based authentication
- Role-based authorization
- Admin, Customer, and Courier roles
- Protected API endpoints

### Customer

- Register and log in
- Create packages
- View personal packages
- View package details
- Track shipments

### Courier

- Register and log in
- View assigned packages
- Update package delivery status
- View shipment tracking information

### Admin

- View customers
- View couriers
- View packages
- Assign packages to couriers
- Manage delivery-related operations

### Tracking

- Unique package tracking numbers
- Package status updates
- Tracking history
- Customer and courier tracking views

### Frontend

The ASP.NET Core application serves a static frontend from `wwwroot/`.

Main pages:

- `index.html` — Login page / application landing page
- `register-v2.html` — Registration page
- `dashboard.html` — Main dashboard
- `forgot-password.html` — Password recovery UI

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | C# / ASP.NET Core 8 Web API |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Authentication | JWT Bearer Authentication |
| API Documentation | Swagger / OpenAPI |
| Frontend | HTML5, CSS3, JavaScript |
| UI | AdminLTE, Bootstrap |
| Development | .NET 8 SDK, Visual Studio / VS Code |

---

## Project Structure

```text
ProjectAPI/
├── Controllers/       # API controllers
├── Data/              # Entity Framework Core DbContext
├── DTOs/              # Request/response data transfer objects
├── Models/            # Database/domain models
├── Services/          # Application services
├── Properties/        # Local launch settings
├── wwwroot/           # Frontend static files
│   ├── index.html
│   ├── register-v2.html
│   ├── dashboard.html
│   ├── forgot-password.html
│   ├── css/
│   ├── js/
│   └── img/
├── Program.cs         # Application configuration and startup
├── appsettings.json   # Non-secret application configuration
├── ProjectAPI.csproj  # .NET project file
├── ProjectAPI.http    # HTTP/API testing requests
└── ProjectAPI.sln     # Visual Studio solution
```

Build artifacts such as `bin/` and `obj/` are intentionally excluded from the repository.

---

## API Overview

The API is organized into the following areas:

- **Auth** — registration and login
- **Admin** — administrative operations
- **Customer** — customer operations
- **Courier** — courier operations
- **Package** — package creation, assignment, status, and lookup
- **Tracking** — tracking records and shipment history
- **Contacts** — contact-related operations

Swagger provides an interactive API reference while the application is running.

```text
/swagger
```

---

## Getting Started

### Prerequisites

Install:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Microsoft SQL Server or SQL Server Express
- SQL Server Management Studio (SSMS) — recommended
- Git — if cloning the repository

Verify the .NET SDK:

```bash
dotnet --version
```

### 1. Clone the repository

```bash
git clone https://github.com/YOUR-USERNAME/YOUR-REPOSITORY.git
cd YOUR-REPOSITORY/ProjectAPI
```

### 2. Configure SQL Server

The default development connection string expects SQL Server Express at:

```text
.\SQLEXPRESS
```

The database name is:

```text
CtmsDB
```

If your SQL Server instance is different, update the connection string accordingly.

### 3. Configure the JWT secret securely

A real JWT secret should **not** be committed to GitHub.

This repository intentionally contains a placeholder in `appsettings.json`.

For local development, use .NET User Secrets:

```bash
dotnet user-secrets set "JwtSettings:SecretKey" "YOUR-DEVELOPMENT-SECRET-KEY"
```

You can also configure the database connection through User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:CtmsConnection" "YOUR-SQL-SERVER-CONNECTION-STRING"
```

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Build

```bash
dotnet build
```

### 6. Run

```bash
dotnet run
```

Open the local URL displayed by ASP.NET Core. The login page is available at the root because it is now named `index.html`.

Swagger is available at:

```text
/swagger
```

---

## Authentication Flow

```text
User
  │
  ├── Register
  │
  └── Login
        │
        ▼
   ASP.NET Core API
        │
        ▼
   Validate credentials
        │
        ▼
     Generate JWT
        │
        ▼
   Frontend stores token
        │
        ▼
Authenticated API requests
```

The frontend sends the JWT in the `Authorization` header for protected requests:

```http
Authorization: Bearer <token>
```

---

## Package Lifecycle

A typical shipment follows a workflow similar to:

```text
Package Created
      ↓
Waiting for Assignment
      ↓
Assigned to Courier
      ↓
In Transit
      ↓
Out for Delivery
      ↓
Delivered
```

---

## Database

Entity Framework Core is used to communicate with Microsoft SQL Server.

The main entities include:

- Admin
- Customer
- Courier
- Package
- Tracking
- Contact

The database context is located at:

```text
Data/CtmsDbContext.cs
```

---

## Testing the API

The API can be tested using:

- Swagger UI
- Postman
- Visual Studio / VS Code `.http` requests
- Browser developer tools for frontend API requests

The repository includes:

```text
ProjectAPI.http
```

for HTTP request testing.

---

## Security Notes

This project is intended for educational purposes and contains some demonstration-oriented authentication code.

For a production system, the following should be improved before deployment:

- Use a strong password hashing algorithm such as ASP.NET Core Identity / PBKDF2 / Argon2 / bcrypt rather than storing plain passwords.
- Remove or disable demonstration endpoints that compare plain-text passwords.
- Store JWT secrets and database credentials outside source control.
- Use HTTPS in production.
- Add proper password reset and email verification workflows.
- Add validation, rate limiting, logging, and auditing appropriate for a production application.

**Never commit real passwords, API keys, JWT secrets, or production database credentials to GitHub.**

---

## Future Improvements

Potential extensions include:

- Real-time courier GPS tracking
- Email/SMS delivery notifications
- Online payment integration
- Proof-of-delivery / digital signatures
- Advanced search and filtering
- Analytics and reporting dashboard
- Automated unit and integration tests
- Production cloud deployment
- Improved password recovery and account verification

---

## Educational Objectives

This project demonstrates practical experience with:

- C# programming
- ASP.NET Core Web API
- RESTful API design
- Entity Framework Core
- SQL Server
- JWT authentication
- Role-based authorization
- CRUD operations
- DTO-based API design
- Frontend/backend integration
- Database-driven application architecture

---

## Author

**Saif Ali**  
Software Engineering Student

---

## License

This project is provided for educational and portfolio purposes.
