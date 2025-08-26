# ThreadPilotTest

## Prerequisites

Make sure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Java](https://www.java.com/download/)
- [Liquibase](https://www.liquibase.com/download-oss) (includes both Java and Liquibase)

---

## Installation & Setup

### 1. Set up PostgreSQL with Docker

Pull the PostgreSQL image:

```bash
docker pull postgres
```

Start a PostgreSQL container:

```bash
docker run --name thread-pilot-db -e POSTGRES_PASSWORD=threadpilotpwd -p 5432:5432 -d postgres
```

### 2. Apply Database Migrations

Navigate to the `DevLiquibase` folder:

```bash
cd src/IF.ThreadPilot/IF.ThreadPilot.Core.Infrastructure/Database/DevLiquibase
```

Run Liquibase to update the schema:

```bash
liquibase update
```

### 3. Start the APIs

Start both **Vehicle** and **Insurance** APIs either:

- In **Visual Studio**
- Or via the CLI:

```bash
dotnet run
```

#### Endpoints

- Vehicle API → [https://localhost:7039/swagger](https://localhost:7039/swagger)
- Insurance API → [https://localhost:7214/swagger](https://localhost:7214/swagger)

---

## Architecture

### General Approach

The solution follows **Clean Architecture** principles:

- **Domain** and **Infrastructure** layers are separated.
- Web applications follow a **feature-based architecture** for maintainability.

### Database-First Strategy

- **PostgreSQL** is used as the primary database.
- A database-first approach was chosen for **BI (Business Intelligence)** readiness and tighter schema control.
- This offers flexibility while giving developers full control over the database design.

### Database Versioning (Liquibase)

- **Liquibase** manages schema versioning and migrations.
- Chosen for reliability and flexibility compared to other tools.

### Reverse POCO

- Generates **POCO entities** from the database tables.
- Provides flexibility similar to **code-first** while leveraging a **database-first** workflow.

### Validation

- Uses **FluentValidation** for input validation.
- One dependency is deprecated but not updated due to time constraints.

### Security

- No authentication/authorization implemented to keep setup simple.
- In production, I would integrate **Azure Entra ID** with **roles/policies**.
- Users would authenticate via **client/secret** or **Azure AD account**.

---

## Personal Reflection

- I previously worked at **VCC** on an application that handled car accidents, integrating with vehicles, SOS services, and insurance companies. The project was complex, involving heavy computations and mathematical operations. To ensure good performance, we leveraged materialized views and other caching strategies, which significantly improved response times and system efficiency.
- The main challenge here was balancing **scalability** with **simplicity**.
- **Performance testing** has not been done. Parallel calls to the Vehicle API may not be optimized.
- The Vehicle API currently accepts **only one registration number at a time**, which may impact performance for customers with multiple vehicles.
  - In production, I would consider **OData** or **GraphQL** with `expand` to fetch related entities efficiently.
- The **database schema** works for demo purposes but is not production-optimized.
- I did not have time to implement a CD pipeline with terraform or bicep for the IaC.
- Many aspects could be further improved in a real-world implementation.

---
