# ModularCore — Enterprise Service Management API

A modular enterprise backend for managing customers, service requests, teams, and service records. Built with Clean Architecture and CQRS using ASP.NET Core 8.

## 🏗 Architecture

This project strictly follows **Clean Architecture** principles with 4 layers:

- **Domain**: Pure business entities (`Customer`, `ServiceRequest`, `Team`) with no dependencies. Contains the state machine logic for `ServiceRequest` status transitions.
- **Application**: CQRS commands and queries using MediatR. Defines `IApplicationDbContext` abstraction.
- **Infrastructure**: Entity Framework Core implementation with PostgreSQL.
- **API**: ASP.NET Core Web API with Swagger, global exception handling, and controllers.

## 🚀 Tech Stack

- **Backend**: C# 12, ASP.NET Core 8
- **CQRS**: MediatR
- **ORM**: Entity Framework Core 8
- **Database**: PostgreSQL 16 (via Docker)
- **Testing**: xUnit, `Microsoft.AspNetCore.Mvc.Testing`, SQLite in-memory
- **Documentation**: Swagger / OpenAPI 3

## ✨ Features Implemented

- **Customer Management**: Create and list customers via CQRS.
- **Service Request Management**: Full lifecycle with business-rule-enforced status transitions.
- **Domain State Machine**: `Closed` requests cannot be reopened; `Resolved → Open` is blocked.
- **Audit Logging**: Every status change automatically creates an audit entry.
- **Comments**: Internal vs. external comments on service requests.
- **Search & Filter**: Paginated search with status filtering.
- **Global Exception Handling**: `DomainValidationException` → `400`, `NotFoundException` → `404`, `InvalidStatusTransitionException` → `409`.
- **Automated Tests**: Domain unit tests + API integration tests using in-memory SQLite.

## 📂 Project Structure

```text
modularcore/
├── src/
│   ├── ModularCore.Domain/          # Entities, Enums, Domain Exceptions
│   ├── ModularCore.Application/     # CQRS Commands/Queries, Interfaces
│   ├── ModularCore.Infrastructure/  # EF Core DbContext, DI Registration
│   └── ModularCore.API/             # Controllers, Middleware, Program.cs
├── tests/
│   └── ModularCore.Tests/           # xUnit Unit + Integration Tests
├── docker-compose.yml
└── ModularCore.sln