# ProjectSpace

ProjectSpace is a full-stack project and task management application built with **ASP.NET Core**, **Entity Framework Core**, and **JWT Authentication**.  
The goal of this project is to provide a secure backend API for managing projects and tasks, where each authenticated user can manage their own workspace.

This repository currently contains the **backend API**, which handles authentication and will support project and task management features.

---

## Tech Stack

**Backend**
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity
- JWT Bearer Authentication
- SQL Server

**Architecture Concepts**
- RESTful API design
- DTO-based request handling
- Secure authentication using JWT
- User-scoped data access

---

## Authentication

Authentication is implemented using **ASP.NET Core Identity** and **JWT tokens**.

### Current Auth Endpoints

| Method | Endpoint | Description |
|------|------|------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and receive JWT token |
| GET | `/api/auth/me` | Get current authenticated user |

Protected endpoints require a JWT token in the header:
Authorization: Bearer <your_token_here>


---

## Current Project Status

Implemented:

- User registration
- User login
- JWT token generation
- Authenticated user endpoint
- Identity-based user management
- Database integration using EF Core

Planned next:

- Project CRUD endpoints
- Task CRUD endpoints
- User-scoped project access
- Task filtering and status updates
- Frontend integration (Next.js)

---

## Project Structure
ProjectSpace
│
├── Controllers
│ └── AuthController.cs
│
├── Data
│ └── AppDbContext.cs
│
├── Models
│ ├── ApplicationUser.cs
│ ├── Project.cs
│ └── TaskItem.cs
│
├── Dtos
│ └── Auth
│ ├── RegisterDto.cs
│ └── LoginDto.cs
│
├── Enums
│ ├── ProjectStatus.cs
│ └── TaskStatus.cs
│
└── Program.cs


---

## Setup Instructions

1. Clone the repository
git clone https://github.com/yourusername/projectspace.git

2. Configure database connection in `appsettings.json`
"ConnectionStrings": {
"DefaultConnection": "your_sql_server_connection"
}

3. Run migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

4. Run the API
dotnet run

5. Open API documentation
/scalar/v1


---

## Goal of the Project

This project is designed to practice and demonstrate:

- ASP.NET Core Identity
- JWT-based authentication
- Secure API development
- EF Core relationships and data modeling
- Full-stack integration with modern frontend frameworks

---

## Future Improvements

- Role-based authorization
- Project collaboration features
- Task assignment
- Pagination and filtering
- Activity logs
- Frontend UI (Next.js + Tailwind + shadcn/ui)

---

## 📄 License

This project is for educational and learning purposes.

