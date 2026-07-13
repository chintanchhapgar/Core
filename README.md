<p align="center">
  <img src="docs/images/banner.png" alt="ShopSphere Banner"/>
</p>
<h1 align="center">🔗 URL Shortener API</h1>

<p align="center">
  <em>Enterprise-grade URL Shortening REST API — ASP.NET Core 8 · Clean Architecture · CQRS · MediatR · EF Core · JWT · Serilog · GitHub Actions</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/ASP.NET_Core-8-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white"/>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/EF_Core-8.0-68217A?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/Auth-JWT-F6A623?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/CQRS-MediatR-38A169?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/Architecture-Clean-007ACC?style=for-the-badge&logoColor=white"/>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Tests-xUnit-2B7BB9?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/Assertions-FluentAssertions-38A169?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/Logging-Serilog-CC2927?style=for-the-badge&logoColor=white"/>
  <img src="https://img.shields.io/badge/CI%2FCD-GitHub_Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white"/>
</p>

<p align="center">
  <img src="https://github.com/chintanchhapgar/Core/actions/workflows/dotnet.yml/badge.svg" alt="Build Status"/>
  &nbsp;
  <img src="https://img.shields.io/github/license/chintanchhapgar/Core?style=for-the-badge" alt="License"/>
  &nbsp;
  <img src="https://img.shields.io/github/last-commit/chintanchhapgar/Core?style=for-the-badge" alt="Last Commit"/>
  &nbsp;
  <img src="https://img.shields.io/github/repo-size/chintanchhapgar/Core?style=for-the-badge" alt="Repo Size"/>
</p>

---

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Architecture](#architecture)
- [Solution Structure](#solution-structure)
- [Technology Stack](#technology-stack)
- [API Endpoints](#api-endpoints)
- [Security](#security)
- [Performance](#performance)
- [Health Checks](#health-checks)
- [Testing](#testing)
- [Running Locally](#running-locally)
- [Configuration](#configuration)
- [CI/CD](#cicd)
- [Screenshots](#screenshots)
- [Roadmap](#roadmap)
- [Engineering Practices](#engineering-practices)
- [License](#license)
- [Author](#author)

---

## Overview

The **URL Shortener API** is a production-ready, enterprise-grade RESTful service built with **ASP.NET Core 8**, following **Clean Architecture**, **CQRS**, and **Domain-Driven Design** principles.

This project serves as a comprehensive reference implementation demonstrating modern backend engineering practices — from JWT Authentication and CQRS with MediatR, to distributed caching, background services, comprehensive integration testing, and automated CI/CD pipelines.

---

## Key Features

| Domain | Capabilities |
|---|---|
| **Authentication & Identity** | JWT Bearer Tokens · User Registration · User Login · Protected Endpoints · Claims-based Access |
| **URL Management** | Create Short URLs · Custom Aliases · Update URLs · Delete URLs · URL Expiration |
| **URL Lifecycle** | Activate URLs · Deactivate URLs · User-specific URL Management |
| **Redirect Engine** | Fast URL Resolution · Click Tracking · Visit Logging · Expiry & Status Validation |
| **Analytics** | Total Click Count · Browser Statistics · OS Statistics · Recent Visits · Analytics Caching |
| **Performance** | In-Memory Cache · URL Cache · Analytics Cache · Optimized Queries · Pagination |
| **Reliability** | Health Checks · Background Cleanup Service · Global Exception Handling · Correlation IDs |
| **Observability** | Serilog Structured Logging · Health Endpoints · Rate Limiting |
| **Testing** | 29+ Integration Tests · xUnit · FluentAssertions · SQLite In-Memory Testing |
| **DevOps** | GitHub Actions CI · Automated Build, Restore & Test Pipeline |

---

## Architecture

The system strictly adheres to **Clean Architecture** with enforced layer boundaries and a unidirectional dependency rule.

```mermaid
flowchart TD
    Client["🌐 Client / Browser"]
    API["⚙️ API Layer\nControllers · Middleware · Auth"]
    APP["📋 Application Layer\nCQRS · MediatR · Validation · Use Cases"]
    DOMAIN["🏛 Domain Layer\nEntities · Aggregates · Business Rules"]
    INFRA["🔧 Infrastructure\nLogging · Caching · Background Services"]
    PERSIST["🗃 Persistence\nEF Core · Repositories · Unit of Work"]
    DB[("🗄️ SQL Server / SQLite")]

    Client --> API
    API --> APP
    APP --> DOMAIN
    APP --> INFRA
    APP --> PERSIST
    PERSIST --> DB

    classDef api fill:#2563EB,color:#ffffff,stroke:#1E40AF,stroke-width:2px
    classDef app fill:#059669,color:#ffffff,stroke:#047857,stroke-width:2px
    classDef domain fill:#7C3AED,color:#ffffff,stroke:#6D28D9,stroke-width:2px
    classDef infra fill:#EA580C,color:#ffffff,stroke:#C2410C,stroke-width:2px
    classDef db fill:#1F2937,color:#ffffff,stroke:#111827,stroke-width:2px

    class API api
    class APP app
    class DOMAIN domain
    class INFRA,PERSIST infra
    class DB db
```

### Request Flow

```mermaid
flowchart TD
    A["🌐 Client"]
    B["🚀 API Endpoint\n(Controller)"]
    C["📨 MediatR Dispatcher"]
    D["⚡ Command / Query"]
    E["🧠 Handler\n(Business Logic)"]
    F["📦 Repository\n(Abstraction)"]
    G["🔧 Entity Framework Core"]
    H[("💾 Database")]

    A --> B
    B --> C
    C --> D
    D --> E
    E --> F
    F --> G
    G --> H

    classDef api fill:#2563EB,color:#ffffff,stroke:#1E40AF,stroke-width:2px
    classDef app fill:#059669,color:#ffffff,stroke:#047857,stroke-width:2px
    classDef infra fill:#EA580C,color:#ffffff,stroke:#C2410C,stroke-width:2px
    classDef db fill:#7C3AED,color:#ffffff,stroke:#6D28D9,stroke-width:2px

    class B api
    class C,D,E app
    class F,G infra
    class H db
```

> **Dependency Rule:** Dependencies flow strictly inward. The Domain layer has zero external dependencies. Application depends only on Domain. Infrastructure and Persistence depend on Application — never the reverse.

---

## Solution Structure

```text
UrlShortener/
│
├── src/
│   ├── UrlShortener.API              # Presentation — Controllers, Middleware, Configuration
│   ├── UrlShortener.Application      # Use Cases — CQRS Commands, Queries, Handlers, Validators
│   ├── UrlShortener.Domain           # Core Business — Entities, Aggregates, Domain Rules
│   ├── UrlShortener.Infrastructure   # Cross-cutting — Logging, Caching, Background Services
│   └── UrlShortener.Persistence      # Data — EF Core Context, Repositories, Migrations
│
├── tests/
│   ├── UrlShortener.UnitTests        # Domain & Business Logic Tests
│   └── UrlShortener.IntegrationTests # End-to-End API Tests (SQLite In-Memory)
│
└── UrlShortener.sln
```

---

## Technology Stack

| Category | Technology |
|---|---|
| **Framework** | ASP.NET Core 8 |
| **Language** | C# 12 |
| **ORM** | Entity Framework Core 8 |
| **Database** | SQL Server / SQLite |
| **Architecture** | Clean Architecture |
| **Design Patterns** | CQRS · MediatR · Repository · Unit of Work |
| **Authentication** | ASP.NET Identity · JWT Bearer |
| **Validation** | FluentValidation |
| **Logging** | Serilog |
| **Caching** | IMemoryCache |
| **Background Services** | .NET Hosted Services |
| **API Documentation** | Swagger / OpenAPI |
| **Testing** | xUnit · FluentAssertions · SQLite In-Memory |
| **CI/CD** | GitHub Actions |

---

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|:---:|---|---|
| `POST` | `/api/auth/register` | Register a new user account |
| `POST` | `/api/auth/login` | Authenticate and receive JWT token |

### URL Management

| Method | Endpoint | Description |
|:---:|---|---|
| `POST` | `/api/urls` | Create a new shortened URL |
| `GET` | `/api/urls` | Retrieve all URLs for authenticated user |
| `PUT` | `/api/urls/{id}` | Update an existing URL |
| `DELETE` | `/api/urls/{id}` | Delete a URL permanently |
| `PUT` | `/api/urls/{id}/activate` | Activate a deactivated URL |
| `PUT` | `/api/urls/{id}/deactivate` | Deactivate an active URL |
| `GET` | `/api/urls/{id}/analytics` | Retrieve analytics for a specific URL |

### Redirect

| Method | Endpoint | Description |
|:---:|---|---|
| `GET` | `/{shortCode}` | Resolve and redirect to the original URL |

### Health

| Method | Endpoint | Description |
|:---:|---|---|
| `GET` | `/health` | Returns system health status |

---

## Security

- ✅ JWT Bearer Token Authentication
- ✅ Claims-based Authorization Policies
- ✅ Secure Password Hashing (ASP.NET Identity)
- ✅ Protected Endpoints with `[Authorize]`
- ✅ API Rate Limiting to prevent abuse
- ✅ Secure Middleware Pipeline
- ✅ Global Exception Handling — no stack traces exposed

---

## Performance

- ✅ In-Memory URL Caching for fast redirect resolution
- ✅ Analytics Result Caching to reduce database load
- ✅ Background Cleanup Service for expired URL removal
- ✅ Optimized EF Core Queries with AsNoTracking
- ✅ Pagination support for large datasets

---

## Health Checks

The application exposes a health endpoint for infrastructure monitoring:

```
GET /health
```

| Check | Description |
|---|---|
| **Database** | Validates SQL Server / SQLite connectivity |
| **Memory** | Monitors application memory pressure |
| **Cache** | Validates IMemoryCache availability |

---

## Testing

The project includes a comprehensive automated test suite:

| Type | Count | Coverage |
|---|:---:|---|
| Integration Tests | 29+ | Auth, CRUD, Redirect, Analytics, Health |
| Unit Tests | — | Domain Logic, Validators |

Tests use **SQLite In-Memory** for fast, isolated database testing without external dependencies.

Run all tests:

```bash
dotnet test
```

---

## Running Locally

**1. Clone the Repository**

```bash
git clone https://github.com/chintanchhapgar/core.git
cd UrlShortener
```

**2. Restore Dependencies**

```bash
dotnet restore
```

**3. Build the Solution**

```bash
dotnet build
```

**4. Run the API**

```bash
dotnet run --project src/UrlShortener.API
```

**5. Access Swagger UI**

```
https://localhost:5001/swagger
```

---

## Configuration

Update `appsettings.json` with your environment values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Database_Connection_String"
  },
  "Jwt": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "SecretKey": "your-secret-key-minimum-32-characters"
  },
  "Serilog": {
    "MinimumLevel": "Information"
  }
}
```

---

## CI/CD

GitHub Actions automatically runs on every **push** and **pull request**:

```mermaid
flowchart LR
    A["📦 Push / PR"] --> B["🔄 Restore Packages"]
    B --> C["🔨 Build Solution"]
    C --> D["🧪 Run All Tests"]
    D --> E{"✅ Pass / ❌ Fail"}
```

| Step | Description |
|---|---|
| **Restore** | Restores all NuGet packages |
| **Build** | Compiles the entire solution |
| **Test** | Executes all unit and integration tests |

---

## Screenshots

<p align="center">
  <img width="750" src="https://github.com/user-attachments/assets/6f9c9100-912a-42e2-a557-fea2517959b3" alt="Swagger UI"/>
  <br/>
  <em>Swagger API Documentation</em>
</p>

<br/>

<p align="center">
  <img width="750" src="https://github.com/user-attachments/assets/049fb8ec-d077-497f-8c0d-778e75c7e5f3" alt="Health Check"/>
  <br/>
  <em>System Health Monitoring</em>
</p>

<br/>

<p align="center">
  <img width="750" src="https://github.com/user-attachments/assets/632dbdd4-0333-4fe1-af68-309e091e9d07" alt="Analytics"/>
  <br/>
  <em>URL Analytics Dashboard</em>
</p>

<br/>

<p align="center">
  <img width="750" src="https://github.com/user-attachments/assets/11d59bce-7583-4d2a-81fa-b4b5f2f6f71a" alt="GitHub Actions"/>
  <br/>
  <em>GitHub Actions CI/CD Pipeline</em>
</p>

---

## Roadmap

| Feature | Status |
|---|:---:|
| Docker Support | 📅 Planned |
| Redis Distributed Cache | 📅 Planned |
| Refresh Token Support | 📅 Planned |
| PostgreSQL Support | 📅 Planned |
| Azure Deployment | 📅 Planned |
| Kubernetes Support | 📅 Planned |
| OpenTelemetry Integration | 📅 Planned |
| Prometheus Metrics | 📅 Planned |
| Email Verification | 📅 Planned |

---

## Engineering Practices

- ✅ Clean Architecture with strict layer separation
- ✅ CQRS Pattern (Commands & Queries via MediatR)
- ✅ Repository & Unit of Work Pattern
- ✅ Domain-Driven Design (DDD) concepts
- ✅ Dependency Injection throughout
- ✅ SOLID Principles
- ✅ JWT Authentication & Authorization Policies
- ✅ Background Services for automated cleanup
- ✅ In-Memory Caching Strategy
- ✅ Structured Logging & Observability (Serilog)
- ✅ Global Exception Handling Middleware
- ✅ API Rate Limiting
- ✅ Health Check Endpoints
- ✅ Integration & Unit Testing
- ✅ Automated CI/CD via GitHub Actions

---

## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## Author

<p align="center">
  <strong>Chintan Chhapgar</strong>
  <br/><br/>
  <a href="https://github.com/chintanchhapgar">
    <img src="https://img.shields.io/badge/GitHub-chintanchhapgar-181717?style=for-the-badge&logo=github&logoColor=white"/>
  </a>
  &nbsp;
  <a href="https://www.linkedin.com/in/chintanchhapgar/">
    <img src="https://img.shields.io/badge/LinkedIn-chintanchhapgar-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white"/>
  </a>
</p>

---

<p align="center">
  <sub>Built with precision · Engineered for scale · Designed for clarity</sub>
</p>
