# Pathway

**Pathway** is a full-stack online learning platform built with **ASP.NET Core MVC**, connecting students, instructors, and administrators in one role-based system. Instructors create and structure courses, students browse and enroll, and admins oversee the entire platform.

---

## Features

### For Students
- Browse courses by category or keyword search
- View course details, including instructor, price, and structure
- One-click enrollment with duplicate-enrollment protection
- Access full course content (sections, lessons, videos) once enrolled
- Personal dashboard ("My Courses") to track and manage enrollments
- Cancel an enrollment at any time

### For Instructors
- Create, edit, and delete their own courses
- Add a thumbnail image to make courses stand out
- Organize course content into sections and lessons
- Embed YouTube videos in lessons — any link format (`watch?v=`, `youtu.be`, `/shorts/`) is automatically converted for playback
- Full ownership-based access control — instructors can only manage their own courses

### For Admins
- Platform-wide dashboard with key metrics (users, courses, enrollments)
- Manage all users (view, edit, delete) with role badges
- Manage categories and courses across the platform
- Statistics page: average course price, enrollment trends, courses per category, top instructors

### Platform-wide
- Cookie-based authentication with role-based authorization (Student / Instructor / Admin)
- Access-control checks at the service layer to prevent unauthorized data access (IDOR protection)
- Clean, consistent, responsive UI design system across every page

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8) |
| Database | SQL Server |
| ORM | Entity Framework Core (Code-First) |
| Auth | Cookie Authentication + Role-based Authorization |
| Frontend | Razor Views, Bootstrap 5, custom CSS design system |

---

## Architecture

The project follows a layered architecture to keep concerns separated and the codebase testable:

```
Controller  →  Service (business logic)  →  Repository (data access)  →  EF Core  →  SQL Server
```

- **Models** — EF Core entities that map directly to database tables
- **Repositories** — pure data-access layer (CRUD operations)
- **Services** — business logic, validation, and authorization checks
- **ViewModels** — shape data specifically for each view, keeping sensitive/internal fields out of the UI
- **Controllers** — handle HTTP requests and return views

---

## Project Structure

```
Pathway/
├── Controllers/       # MVC controllers (Account, Courses, CourseSections, Lessons, Enrollments, Categories, Admin)
├── Models/             # EF Core entities (User, Course, CourseSection, Lesson, Enrollment, Category)
├── ViewModels/         # View-specific data-transfer models
├── Repositories/        # Data access layer + interfaces
├── Services/            # Business logic layer + interfaces
├── Views/               # Razor views, organized by controller
├── Data/                # AppDbContext
├── Migrations/          # EF Core migrations
└── wwwroot/             # Static assets (CSS, JS, images)
```

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full instance)
- Visual Studio 2022 (recommended) or VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Pathway
   ```

2. **Configure the database connection**

   Update `appsettings.json` with your own SQL Server connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=Pathway;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Apply migrations**

   In Visual Studio's Package Manager Console:
   ```powershell
   Update-Database
   ```
   Or via the .NET CLI:
   ```bash
   dotnet ef database update
   ```

4. **Run the project**
   ```bash
   dotnet run
   ```
   Or press `F5` in Visual Studio.

5. Open the app in your browser, register accounts for each role (Student / Instructor), and explore.

> **Note:** Never commit real database credentials to a public repository. Replace the connection string in `appsettings.json` with a placeholder before pushing, and keep real secrets in a local `appsettings.Development.json` or User Secrets instead.

---

## User Roles

| Role | Access |
|---|---|
| **Student** | Browse, enroll, view enrolled course content |
| **Instructor** | Create/manage own courses, sections, and lessons |
| **Admin** | Full platform management: users, courses, categories, statistics |

---

## License

This project was built as an academic project. Feel free to explore the code for learning purposes.
