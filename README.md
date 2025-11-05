# Course Registration API

A comprehensive RESTful API for managing a course registration system, built with .NET 9.0 and Entity Framework Core. This API provides complete functionality for managing students, courses, instructors, departments, registrations, and users.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Domain Models](#domain-models)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Data Seeding](#data-seeding)
- [Development](#development)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)

## 🎯 Overview

The Course Registration API is a full-featured backend service designed to handle all aspects of a course registration system. It implements best practices including:

- **Repository Pattern** for data access abstraction
- **RESTful API Design** with proper HTTP methods and status codes
- **Entity Framework Core** with In-Memory Database for development
- **Swagger/OpenAPI** for interactive API documentation
- **Dependency Injection** for loose coupling
- **CORS Support** for frontend integration

## ✨ Features

### Core Functionality
- **Student Management**: Complete CRUD operations for student records
- **Course Management**: Create, update, and manage course offerings
- **Instructor Management**: Handle instructor information and assignments
- **Department Management**: Organize courses and instructors by department
- **Registration System**: Register students for courses with status tracking
- **User Management**: User authentication and role-based access preparation

### Advanced Features
- **Automatic Data Seeding**: Pre-populated sample data for testing
- **Rich Domain Models**: Computed properties for enrollment statistics
- **Relationship Management**: Proper foreign keys and navigation properties
- **Status Tracking**: Registration status (Pending, Enrolled, Dropped, Completed)
- **Grading System**: Support for course grades (A, B, C, D, F)
- **Capacity Management**: Track course enrollment vs. maximum capacity
- **Health Checks**: Built-in health check endpoint
- **API Information**: Endpoint providing API metadata

## 🛠 Technology Stack

- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core 9.0**: ORM for data access
- **EF Core In-Memory Database**: Development database provider
- **Swashbuckle (Swagger) 9.0**: API documentation
- **C# 13**: Programming language with nullable reference types

### NuGet Packages
- `Microsoft.AspNetCore.OpenApi` (9.0.8)
- `Microsoft.EntityFrameworkCore.InMemory` (9.0.10)
- `Swashbuckle.AspNetCore` (9.0.6)

## 🏗 Architecture

The application follows a **layered architecture** pattern:

```
CourseRegistrationAPI/
├── Models/              # Domain entities
├── DTOs/                # Data Transfer Objects
├── Data/                # Database context and seeding
├── Repositories/        # Data access layer (Repository pattern)
├── Controllers/         # API endpoints (Presentation layer)
└── Program.cs          # Application configuration
```

### Design Patterns
1. **Repository Pattern**: Abstracts data access logic
2. **Dependency Injection**: Loose coupling between components
3. **DTO Pattern**: Separates domain models from API contracts
4. **Generic Repository**: Reusable data access methods

## 📁 Project Structure

```
CourseRegistrationAPI/
│
├── Controllers/
│   ├── CoursesController.cs          (406 lines)
│   ├── DepartmentsController.cs      (247 lines)
│   ├── InstructorsController.cs      (291 lines)
│   ├── RegistrationsController.cs    (343 lines)
│   ├── StudentsController.cs         (228 lines)
│   └── UsersController.cs            (396 lines)
│
├── Models/
│   ├── Course.cs                     (63 lines)
│   ├── Department.cs                 (24 lines)
│   ├── Enums.cs                      (22 lines)
│   ├── Instructor.cs                 (47 lines)
│   ├── Registration.cs               (45 lines)
│   ├── Student.cs                    (44 lines)
│   └── User.cs                       (54 lines)
│
├── DTOs/
│   ├── CourseDTOs.cs                 (Create/Update/Response DTOs)
│   ├── InstructorDepartmentDTOs.cs   (Instructor and Department DTOs)
│   ├── RegistrationUserDTOs.cs       (Registration and User DTOs)
│   └── StudentDTOs.cs                (Student DTOs)
│
├── Repositories/
│   ├── IRepository.cs                (Generic repository interface)
│   ├── Repository.cs                 (Generic repository implementation)
│   ├── ICourseRepository.cs
│   ├── CourseRepository.cs
│   ├── IStudentRepository.cs
│   ├── StudentRepository.cs
│   ├── ISpecificRepositories.cs
│   ├── InstructorDepartmentRepositories.cs
│   └── RegistrationUserRepositories.cs
│
├── Data/
│   ├── CourseRegistrationContext.cs  (EF Core DbContext)
│   └── DataSeeder.cs                 (Sample data seeding)
│
├── Program.cs                        (Application startup)
├── appsettings.json                  (Configuration)
└── CourseRegistrationAPI.csproj      (Project file)
```

## 📊 Domain Models

### 1. Student
Represents students enrolled in the system.
```csharp
- Id (int): Primary key
- FirstName (string, required, max 50)
- LastName (string, required, max 50)
- Email (string, required, email format, max 100)
- DateOfBirth (DateTime, required)
- EnrollmentDate (DateTime, required)
- Phone (string, optional, max 15)
- Address (string, optional, max 200)
- Computed: FullName, Age
```

### 2. Course
Represents course offerings.
```csharp
- Id (int): Primary key
- Title (string, required, max 200)
- Description (string, required, max 1000)
- Credits (int, required, range 1-6)
- MaxCapacity (int, required, range 1-500)
- StartDate (DateTime, required)
- EndDate (DateTime, required)
- InstructorId (int, required, FK)
- DepartmentId (int, required, FK)
- CreatedDate (DateTime)
- ModifiedDate (DateTime)
- IsActive (bool, default true)
- Computed: CurrentEnrollment, AvailableSpots, IsFull, DurationInDays
```

### 3. Instructor
Represents course instructors/faculty.
```csharp
- Id (int): Primary key
- FirstName (string, required, max 50)
- LastName (string, required, max 50)
- Email (string, required, email format, max 100)
- Department (string, required, max 100)
- HireDate (DateTime, required)
- Phone (string, optional, max 15)
- Bio (string, optional, max 500)
- Computed: FullName, YearsOfExperience
```

### 4. Department
Represents academic departments.
```csharp
- Id (int): Primary key
- Name (string, required, max 100)
- Description (string, required, max 500)
- HeadOfDepartment (string, required, max 100)
```

### 5. Registration
Links students to courses with enrollment status.
```csharp
- Id (int): Primary key
- StudentId (int, required, FK)
- CourseId (int, required, FK)
- RegistrationDate (DateTime, required)
- Status (RegistrationStatus enum)
- Grade (Grade? enum, nullable)
- CompletionDate (DateTime?, nullable)
- Notes (string, optional, max 500)
- Computed: IsActive, StatusDescription, GradeDescription
```

### 6. User
Represents system users with authentication.
```csharp
- Id (int): Primary key
- Username (string, required, unique, max 50)
- Email (string, required, unique, email format, max 100)
- PasswordHash (string, required, max 500)
- Role (UserRole enum)
- IsActive (bool, default true)
- CreatedDate (DateTime)
- LastLoginDate (DateTime?, nullable)
- Computed: IsAdmin, IsInstructor, IsStudent, DaysSinceLastLogin
```

### Enumerations

**RegistrationStatus**
- `Pending`: Initial registration state
- `Enrolled`: Successfully enrolled
- `Dropped`: Student dropped the course
- `Completed`: Course completed

**Grade**
- `A`: Excellent (4.0)
- `B`: Good (3.0)
- `C`: Average (2.0)
- `D`: Below Average (1.0)
- `F`: Fail (0.0)

**UserRole**
- `Admin`: System administrator
- `Instructor`: Faculty member
- `Student`: Enrolled student

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Visual Studio 2022 or VS Code with C# extension
- Git (for version control)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/CanarysPlayground/Courseregistrationfeature.git
cd Courseregistrationfeature
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Build the project**
```bash
dotnet build
```

4. **Run the application**
```bash
cd CourseRegistrationAPI
dotnet run
```

The application will start and display:
```
=== Course Registration API Started ===
Environment: Development
Swagger UI: https://localhost:7000 (when running in development)
Health Check: /health
API Info: /api/info
========================================
```

5. **Access the API**
   - **Swagger UI**: Open your browser to `https://localhost:7000` or `http://localhost:5000`
   - **API Endpoints**: Base URL is `https://localhost:7000/api`
   - **Health Check**: `https://localhost:7000/health`
   - **API Info**: `https://localhost:7000/api/info`

## 🔌 API Endpoints

### Students API (`/api/students`)
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student
- `GET /api/students/{id}/registrations` - Get student's registrations

### Courses API (`/api/courses`)
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create new course
- `PUT /api/courses/{id}` - Update course
- `DELETE /api/courses/{id}` - Delete course
- `GET /api/courses/{id}/registrations` - Get course registrations
- `GET /api/courses/instructor/{instructorId}` - Get courses by instructor
- `GET /api/courses/department/{departmentId}` - Get courses by department

### Instructors API (`/api/instructors`)
- `GET /api/instructors` - Get all instructors
- `GET /api/instructors/{id}` - Get instructor by ID
- `POST /api/instructors` - Create new instructor
- `PUT /api/instructors/{id}` - Update instructor
- `DELETE /api/instructors/{id}` - Delete instructor
- `GET /api/instructors/{id}/courses` - Get instructor's courses

### Departments API (`/api/departments`)
- `GET /api/departments` - Get all departments
- `GET /api/departments/{id}` - Get department by ID
- `POST /api/departments` - Create new department
- `PUT /api/departments/{id}` - Update department
- `DELETE /api/departments/{id}` - Delete department
- `GET /api/departments/{id}/courses` - Get department's courses
- `GET /api/departments/{id}/instructors` - Get department's instructors

### Registrations API (`/api/registrations`)
- `GET /api/registrations` - Get all registrations
- `GET /api/registrations/{id}` - Get registration by ID
- `POST /api/registrations` - Create new registration
- `PUT /api/registrations/{id}` - Update registration
- `DELETE /api/registrations/{id}` - Delete registration
- `GET /api/registrations/student/{studentId}` - Get student's registrations
- `GET /api/registrations/course/{courseId}` - Get course's registrations
- `PUT /api/registrations/{id}/status` - Update registration status

### Users API (`/api/users`)
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `GET /api/users/username/{username}` - Get user by username
- `GET /api/users/email/{email}` - Get user by email
- `GET /api/users/role/{role}` - Get users by role

### System Endpoints
- `GET /health` - Health check endpoint
- `GET /api/info` - API information and available endpoints

## 🌱 Data Seeding

The application automatically seeds the database with sample data on startup, including:

### Sample Departments
- Computer Science
- Mathematics
- Physics
- Business Administration

### Sample Instructors
- 8 instructors across different departments
- Complete profile information

### Sample Students
- 15 students with varied enrollment dates
- Complete contact and personal information

### Sample Courses
- 12 courses across all departments
- Realistic course details, schedules, and capacities
- Examples: "Introduction to Programming", "Data Structures", "Calculus I", "Linear Algebra", etc.

### Sample Registrations
- 25+ registration records
- Various statuses (Enrolled, Completed, Pending, Dropped)
- Some with grades assigned

### Sample Users
- Admin, Instructor, and Student users
- Ready for authentication implementation

## 💻 Development

### Building the Project
```bash
dotnet build
```

### Running in Development Mode
```bash
dotnet run --environment Development
```

### Configuration
Edit `appsettings.json` or `appsettings.Development.json` for:
- Logging levels
- CORS policies
- Application settings

### Database
The application uses **Entity Framework Core In-Memory Database**, which:
- Resets on each application restart
- Is perfect for development and testing
- Requires no setup or configuration
- Automatically seeds with sample data

### CORS Configuration
Currently configured to allow any origin for development:
```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```
**Note**: Tighten CORS policy for production deployment.

## 📚 API Documentation

### Swagger/OpenAPI
The API includes comprehensive Swagger documentation:
- **Interactive API Testing**: Try endpoints directly from the browser
- **Schema Documentation**: View request/response models
- **Authentication Ready**: Prepared for JWT integration

Access Swagger UI at the application root when running in development mode.

### Response Format
All API responses follow standard HTTP conventions:
- `200 OK` - Successful GET, PUT requests
- `201 Created` - Successful POST requests
- `204 No Content` - Successful DELETE requests
- `400 Bad Request` - Validation errors
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server errors

### Sample Request/Response

**Create Student (POST /api/students)**

Request:
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "dateOfBirth": "2000-01-15",
  "enrollmentDate": "2024-09-01",
  "phone": "123-456-7890",
  "address": "123 Main St"
}
```

Response (201 Created):
```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "fullName": "John Doe",
  "email": "john.doe@example.com",
  "dateOfBirth": "2000-01-15T00:00:00",
  "enrollmentDate": "2024-09-01T00:00:00",
  "phone": "123-456-7890",
  "address": "123 Main St",
  "age": 24
}
```

## 🎯 Key Features Explained

### Repository Pattern
The application implements the Repository pattern for clean data access:
- **Generic Repository**: Base CRUD operations (`IRepository<T>`, `Repository<T>`)
- **Specific Repositories**: Entity-specific operations (e.g., `ICourseRepository`)
- **Dependency Injection**: Repositories injected into controllers

### Computed Properties
Domain models include computed properties for convenience:
- `Student.FullName`: Concatenated first and last name
- `Student.Age`: Calculated from date of birth
- `Course.CurrentEnrollment`: Count of enrolled students
- `Course.AvailableSpots`: Remaining capacity
- `Course.IsFull`: Boolean indicating if at capacity
- `Instructor.YearsOfExperience`: Calculated from hire date

### Navigation Properties
Proper EF Core relationships:
- `Course.Instructor` and `Course.Department`
- `Registration.Student` and `Registration.Course`
- `Student.Registrations` and `Course.Registrations`

## 🔐 Security Considerations

**Current State**: Basic API without authentication
**Future Enhancements** (see `.github/instructions/Security-Standards.instructions.md`):
- JWT authentication implementation
- Role-based authorization
- Input validation with FluentValidation
- Password hashing (never store plaintext)
- HTTPS enforcement
- Rate limiting
- Security headers
- Audit logging

## 🤝 Contributing

This is a learning and demonstration project. Feel free to:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

### Development Standards
Follow the guidelines in:
- `.github/instructions/dotnet-dev.instructions.md`
- `.github/instructions/Security-Standards.instructions.md`

## 📄 License

This project is provided as-is for educational purposes.

## 🙏 Acknowledgments

Built with modern .NET best practices including:
- Repository Pattern
- Dependency Injection
- RESTful API Design
- Entity Framework Core
- Swagger/OpenAPI Documentation

## 📞 Contact

For questions or support, please open an issue in the GitHub repository.

---

**Happy Coding! 🚀**
