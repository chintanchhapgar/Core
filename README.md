# 🔗 URL Shortener API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0-purple)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)
![CQRS](https://img.shields.io/badge/Pattern-CQRS-orange)
![License](https://img.shields.io/badge/License-MIT-green)
![Build](https://github.com/<YOUR_GITHUB_USERNAME>/UrlShortener/actions/workflows/dotnet.yml/badge.svg)

A production-ready **URL Shortener REST API** built with **ASP.NET Core 8** following **Clean Architecture**, **CQRS**, and **Domain-Driven Design** principles.

This project demonstrates modern backend development practices including JWT Authentication, Entity Framework Core, MediatR, Caching, Rate Limiting, Health Checks, Background Services, Integration Testing, and GitHub Actions CI.

---

# 🚀 Features

## Authentication

- JWT Authentication
- User Registration
- User Login
- Protected Endpoints
- Current User Context

## URL Management

- Create Short URLs
- Custom Aliases
- Update URLs
- Delete URLs
- Activate URLs
- Deactivate URLs
- URL Expiration
- User-specific URLs

## Redirect

- Fast URL Resolution
- Click Tracking
- Visit Logging
- Expired URL Validation
- Inactive URL Validation

## Analytics

- Total Click Count
- Browser Statistics
- Operating System Statistics
- Recent Visits
- Analytics Caching

## Infrastructure

- Memory Cache
- Background Cleanup Service
- Health Checks
- Structured Logging (Serilog)
- Global Exception Handling
- Correlation IDs
- Rate Limiting

## Testing & DevOps

- Integration Tests
- xUnit
- FluentAssertions
- GitHub Actions CI
- SQLite In-Memory Testing

---

# 🏗 Architecture

The project follows **Clean Architecture** with clear separation of concerns.

```
                Client
                   │
                   ▼
          ASP.NET Core Web API
                   │
      Authentication / Middleware
                   │
                   ▼
              MediatR (CQRS)
             /             \
        Commands         Queries
             \             /
                   ▼
          Application Layer
                   │
         Repository Pattern
                   │
          Unit of Work Pattern
                   │
                   ▼
            Entity Framework Core
                   │
                   ▼
        SQLite / SQL Server Database
```

---

# 📁 Project Structure

```
UrlShortener
│
├── src
│   ├── UrlShortener.API
│   ├── UrlShortener.Application
│   ├── UrlShortener.Domain
│   ├── UrlShortener.Infrastructure
│   └── UrlShortener.Persistence
│
├── tests
│   ├── UrlShortener.UnitTests
│   └── UrlShortener.IntegrationTests
│
└── UrlShortener.sln
```

---

# 🛠 Technology Stack

| Category | Technology |
|------------|----------------------------|
| Framework | ASP.NET Core 8 |
| Language | C# 12 |
| ORM | Entity Framework Core |
| Database | SQLite / SQL Server |
| Authentication | JWT Bearer |
| Architecture | Clean Architecture |
| Pattern | CQRS + MediatR |
| Validation | FluentValidation |
| Logging | Serilog |
| Caching | IMemoryCache |
| Testing | xUnit |
| Assertions | FluentAssertions |
| CI/CD | GitHub Actions |

---

# 📌 API Endpoints

## Authentication

| Method | Endpoint |
|---------|----------------------------|
| POST | /api/auth/register |
| POST | /api/auth/login |

---

## URLs

| Method | Endpoint |
|---------|-------------------------------|
| POST | /api/urls |
| GET | /api/urls |
| PUT | /api/urls/{id} |
| DELETE | /api/urls/{id} |
| PUT | /api/urls/{id}/activate |
| PUT | /api/urls/{id}/deactivate |
| GET | /api/urls/{id}/analytics |

---

## Redirect

| Method | Endpoint |
|---------|----------------|
| GET | /{shortCode} |

---

## Health

| Method | Endpoint |
|---------|------------|
| GET | /health |

---

# 🔒 Security

- JWT Authentication
- Authorization Policies
- Password Hashing
- Protected Endpoints
- Rate Limiting
- Secure Middleware Pipeline

---

# ⚡ Performance Features

- Memory Cache
- Analytics Cache
- URL Cache
- Background Cleanup Service
- Optimized Database Queries
- Pagination Support

---

# ❤️ Health Checks

The application exposes health endpoints for monitoring.

```
GET /health
```

Checks include:

- Database
- Memory
- Cache

---

# 🧪 Testing

The project includes a comprehensive integration test suite covering:

- Authentication
- URL Creation
- URL Retrieval
- URL Update
- URL Delete
- Activate / Deactivate
- Redirect
- Analytics
- Health Checks

```
29+ Integration Tests
```

Run all tests:

```bash
dotnet test
```

---

# 🚀 Running Locally

## Clone Repository

```bash
git clone https://github.com/chintanchhapgar/core.git
```

## Navigate

```bash
cd UrlShortener
```

## Restore

```bash
dotnet restore
```

## Build

```bash
dotnet build
```

## Run

```bash
dotnet run --project src/UrlShortener.API
```

Swagger:

```
https://localhost:5001/swagger
```

---

# ⚙ Configuration

Update **appsettings.json**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourConnectionString"
  },

  "Jwt": {
    "Issuer": "...",
    "Audience": "...",
    "SecretKey": "..."
  }
}
```

---

# 🔄 CI/CD

GitHub Actions automatically:

- Restore packages
- Build solution
- Execute all tests

Every push and pull request is automatically validated.

---

# 📈 Future Improvements

- Docker Support
- Redis Cache
- Refresh Tokens
- PostgreSQL
- Azure Deployment
- Kubernetes
- OpenTelemetry
- Prometheus Metrics
- Distributed Cache
- Email Verification

---

# 📷 Screenshots

## Swagger UI

> <img width="911" height="814" alt="image" src="https://github.com/user-attachments/assets/6f9c9100-912a-42e2-a557-fea2517959b3" />

## Health Check

> <img width="886" height="526" alt="image" src="https://github.com/user-attachments/assets/049fb8ec-d077-497f-8c0d-778e75c7e5f3" />


## Analytics

> <img width="731" height="480" alt="image" src="https://github.com/user-attachments/assets/632dbdd4-0333-4fe1-af68-309e091e9d07" />


## GitHub Actions

> <img width="1898" height="689" alt="image" src="https://github.com/user-attachments/assets/11d59bce-7583-4d2a-81fa-b4b5f2f6f71a" />


# 🎯 Learning Objectives

This project demonstrates practical experience with:

- Clean Architecture
- SOLID Principles
- CQRS
- MediatR
- Repository Pattern
- Unit of Work
- Entity Framework Core
- JWT Authentication
- ASP.NET Core Middleware
- Background Services
- Health Checks
- Caching
- Integration Testing
- GitHub Actions

---

# 📄 License

This project is licensed under the MIT License.

---

# 👨‍💻 Author

**Chintan Chhapgar**

- GitHub: https://github.com/chintanchhapgar
- LinkedIn: *[(Add your LinkedIn URL)](https://www.linkedin.com/in/chintanchhapgar)*
