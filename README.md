# Personal Blogging Platform API 🚀

A robust, secure, and production-ready RESTful API built with **ASP.NET Core Web API** (.NET 10) for a personal blogging platform. This project features robust JWT authentication, article ownership authorization, advanced filtering, searching, pagination, and a clean architectural design.

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core Web API (.NET 10)
- **Language:** C#
- **Database & ORM:** SQL Server (LocalDB), Entity Framework Core (EF Core)
- **Authentication & Security:** JWT (JSON Web Tokens), ASP.NET Core Identity/Security mechanisms, Secure Password Hashing
- **API Documentation:** Swagger / OpenAPI with JWT Bearer authorization support
- **Architecture:** Repository + Service Pattern, Clean Separation of Concerns

---

## ✨ Core Features

- **User Authentication:**
  - Secure User Registration and Login.
  - JWT-based authentication with strict validation parameters (Issuer, Audience, Lifetime, and Signing Key).

- **Article Management (CRUD):**
  - Full CRUD operations for blog articles.
  - Automatic association of created articles with the authenticated user.

- **Article Ownership & Authorization:**
  - Strict resource ownership checks ensuring users can only update or delete their *own* articles.
  - Proper HTTP status code responses (`401 Unauthorized`, `403 Forbidden`, `404 Not Found`).

- **Advanced Querying Capabilities:**
  - **Filtering:** Filter articles by date ranges (`FromDate`, `ToDate`).
  - **Search:** Search keywords across article titles and content.
  - **Pagination:** Efficient data paging using `PageNumber` and `PageSize` with deterministic ordering.
  - **Sorting:** Flexible sorting options by creation date or title in ascending/descending order.

- **Global Error Handling:**
  - Custom middleware for handling unhandled exceptions safely without leaking sensitive server details or stack traces to clients.

- **API Documentation:**
  - Interactive Swagger UI integrated with JWT Bearer token authentication support.

---

## 📂 Architecture

The project follows a clean and maintainable layer-based structure:

```text
Controllers
    ↓
Services (Business Logic)
    ↓
Repositories (Data Access)
    ↓
EF Core & SQL Server
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/) installed on your machine.
- SQL Server LocalDB or SQL Server instance.

### Installation & Running

1. **Clone the repository:**

```bash
git clone https://github.com/your-username/PersonalBloggingPlatformAPI.git
cd PersonalBloggingPlatformAPI
```

2. **Configure Connection String & JWT Settings:**

Update `appsettings.json` or use User Secrets to set up your database connection string and JWT secret keys:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonalBloggingDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_HERE",
    "Issuer": "PersonalBloggingAPI",
    "Audience": "PersonalBloggingClient"
  }
}
```

3. **Apply Database Migrations:**

```bash
dotnet ef database update
```

4. **Run the Application:**

```bash
dotnet run
```

5. **Explore the API:**

Open your browser and navigate to:

```text
https://localhost:{port}/swagger
```

to test the endpoints via Swagger UI.

---

## 🔒 API Endpoints Overview

| Method | Endpoint | Description | Access |
|---|---|---|---|
| **POST** | `/api/auth/register` | Register a new user | Public |
| **POST** | `/api/auth/login` | Authenticate and get JWT token | Public |
| **GET** | `/api/articles` | Get articles (supports filter, search, paging, sort) | Public |
| **GET** | `/api/articles/{id}` | Get a specific article by ID | Public |
| **POST** | `/api/articles` | Create a new article | Authenticated |
| **PUT** | `/api/articles/{id}` | Update an article (Ownership check enforced) | Authenticated |
| **DELETE** | `/api/articles/{id}` | Delete an article (Ownership check enforced) | Authenticated |

---

## 🛡️ Security Highlights

- **Password Security:** Passwords are never stored in plain text; implemented with secure hashing algorithms.
- **No Client Trust for Identity:** `UserId` is extracted securely from the JWT claims, preventing users from spoofing ownership.
- **Structured Logging & Error Boundaries:** Errors are securely logged on the server while returning clean, standard HTTP error responses to clients.
