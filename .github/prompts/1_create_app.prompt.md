---
mode: agent
---
You are an expert software developer specializing in .NET applications. Your task is to create a Course Registration application using MVC architecture with Repository pattern and in-memory database.

## Task: Create a .NET Course Registration MVC Application with Required Entities

Build a .NET MVC application for course registration management using Entity Framework Core In-Memory database with the Repository pattern.

### Business Domain:
**Course Registration API** - A RESTful API service for managing courses, students, instructors, and registrations.

### Core Entities:
1. **Student** (Id, FirstName, LastName, Email, DateOfBirth, EnrollmentDate, Phone, Address)
2. **Course** (Id, Title, Description, Credits, MaxCapacity, StartDate, EndDate, InstructorId, DepartmentId)
3. **Instructor** (Id, FirstName, LastName, Email, Department, HireDate, Phone, Bio)
4. **Department** (Id, Name, Description, HeadOfDepartment)
5. **Registration** (Id, StudentId, CourseId, RegistrationDate, Status, Grade)
6. **User** (Id, Username, Email, PasswordHash, Role, IsActive, CreatedDate)

### Technical Requirements:
1. **Web API Architecture**: RESTful API controllers with proper HTTP methods
2. **Repository Pattern**: Generic repository with Entity Framework Core
3. **In-Memory Database**: Entity Framework Core In-Memory provider
4. **API Endpoints**: Full CRUD operations for all entities
5. **Entity Relationships**: Proper navigation properties and foreign keys
6. **Data Seeding**: Sample data for testing and development
7. **Swagger Documentation**: API documentation and testing interface

### Deliverables:
- Complete .NET Web API project structure
- Entity models with proper relationships
- Repository pattern implementation
- Database context with in-memory configuration
- API controllers with CRUD endpoints
- Swagger/OpenAPI configuration
- Sample data seeding
**Course Registration System** - A platform for managing courses, students, instructors, and registrations.

### Core Entities:
1. **Student** (Id, FirstName, LastName, Email, DateOfBirth, EnrollmentDate, Phone, Address)
2. **Course** (Id, Title, Description, Credits, MaxCapacity, StartDate, EndDate, InstructorId, DepartmentId)
3. **Instructor** (Id, FirstName, LastName, Email, Department, HireDate, Phone, Bio)
4. **Department** (Id, Name, Description, HeadOfDepartment)
5. **Registration** (Id, StudentId, CourseId, RegistrationDate, Status, Grade)
6. **User** (Id, Username, Email, PasswordHash, Role, IsActive, CreatedDate)

### Technical Requirements:
1. **MVC Architecture**: Models, Views, Controllers with proper separation
2. **Repository Pattern**: Generic repository with Entity Framework Core
3. **In-Memory Database**: Entity Framework Core In-Memory provider
4. **Basic CRUD Operations**: Create, Read, Update, Delete for all entities
5. **Entity Relationships**: Proper navigation properties and foreign keys
6. **Data Seeding**: Sample data for testing and development

### Deliverables:
- Complete .NET MVC project structure
- Entity models with proper relationships
- Repository pattern implementation
- Database context with in-memory configuration
- Basic controllers and views for entity management
- Sample data seeding
- README file with setup and run instructions
- Ensure you give the backend api response which can be used in frontend development.
- make sure you add the necesary controllers for each entity to perform CRUD operations.
-Automatically show the swagger output in Simple Browser: show when the application starts.