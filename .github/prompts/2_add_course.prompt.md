---
mode: agent
---
You are an expert .NET developer specializing in mvc architecture and API development. Your task is to implement the "Add Course" functionality for the Course Registration API application.

## Task: Implement Add Course API Endpoint

Create a comprehensive implementation for adding courses to the Course Registration system. This should follow the established mvc architecture pattern and repository design already implemented in the application.

### Required Implementation Components:

#### 1. Course Entity Enhancement (if not already complete)
- Ensure the Course entity in the Domain layer has all required properties:
  - Id (Primary Key)
  - Title (Required, Max 200 characters)
  - Description (Required, Max 1000 characters)
  - Credits (Required, Range 1-6)
  - MaxCapacity (Required, Min 1, Max 500)
  - StartDate (Required, Future date)
  - EndDate (Required, After StartDate)
  - InstructorId (Required, Foreign Key)
  - DepartmentId (Required, Foreign Key)
  - CreatedDate (Auto-generated)
  - ModifiedDate (Auto-generated)
  - IsActive (Default true)

#### 2. DTOs and Validation Models
Create the following DTOs in the API layer:

**CreateCourseDto.cs**:
```csharp
public class CreateCourseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int InstructorId { get; set; }
    public int DepartmentId { get; set; }
}
```

**CourseResponseDto.cs**:
```csharp
public class CourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string InstructorName { get; set; }
    public string DepartmentName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
```

#### 3. FluentValidation Validators
Implement comprehensive validation:

**CreateCourseDtoValidator.cs**:
- Title: Required, 5-200 characters, no special characters except spaces and hyphens
- Description: Required, 10-1000 characters
- Credits: Required, Range 1-6
- MaxCapacity: Required, Range 1-500
- StartDate: Required, must be in future (at least 7 days from now)
- EndDate: Required, must be after StartDate, minimum course duration 30 days
- InstructorId: Required, must exist in database
- DepartmentId: Required, must exist in database
- Custom validation: Instructor must belong to the specified department
- Custom validation: No scheduling conflicts with instructor's existing courses

#### 4. Business Logic and Service Layer
Implement in the Application layer:

**ICourseService.cs** interface:
```csharp
Task<Result<CourseResponseDto>> CreateCourseAsync(CreateCourseDto createCourseDto);
```

**CourseService.cs** implementation with:
- Business rule validation
- Duplicate course detection (same title, instructor, overlapping dates)
- Instructor availability checking
- Department capacity validation
- Automatic course code generation
- Transaction handling with Unit of Work pattern

#### 5. Repository Layer Enhancements
Enhance the repository with specific methods:

**ICourseRepository.cs**:
```csharp
Task<bool> ExistsByTitleAndInstructorAsync(string title, int instructorId);
Task<bool> HasScheduleConflictAsync(int instructorId, DateTime startDate, DateTime endDate);
Task<IEnumerable<Course>> GetCoursesByInstructorAndPeriodAsync(int instructorId, DateTime startDate, DateTime endDate);
```

#### 6. Controller Implementation
Create robust API endpoint in CoursesController:

**POST /api/v1/courses** endpoint with:
- Proper HTTP status codes (201 Created, 400 Bad Request, 409 Conflict)
- Location header for created resource
- Comprehensive error handling
- Request/response logging
- Authorization (Admin and Instructor roles only)
- Rate limiting consideration
- API documentation with Swagger examples

#### 7. Error Handling and Response Models
Implement custom exceptions:
- `CourseAlreadyExistsException`
- `InstructorScheduleConflictException`
- `DepartmentCapacityExceededException`
- `InstructorDepartmentMismatchException`

#### 8. AutoMapper Configuration
Set up mapping profiles:
- CreateCourseDto → Course
- Course → CourseResponseDto
- Include navigation properties mapping

### Business Rules to Implement:

1. **Uniqueness**: No duplicate courses (same title + instructor + overlapping dates)
2. **Instructor Availability**: Instructor cannot teach overlapping courses
3. **Department Validation**: Instructor must belong to the specified department
4. **Date Validation**: Course duration minimum 30 days, maximum 180 days
5. **Capacity Limits**: Department cannot exceed total capacity limits
6. **Authorization**: Only Admins and Instructors can create courses
7. **Course Code Generation**: Auto-generate unique course codes (e.g., CS101-2024-SPRING)

### Testing Requirements:
Create comprehensive unit tests for:
- Validator tests with various input scenarios
- Service layer tests with mocked dependencies
- Controller tests with various HTTP scenarios
- Repository tests for database operations
- Integration tests for the complete add course flow

### API Documentation:
Provide complete Swagger documentation with:
- Request/response examples
- Error response examples
- Business rule descriptions
- Authentication requirements

### Security Considerations:
- Input sanitization
- SQL injection prevention (parameterized queries)
- Authorization checks
- Rate limiting for course creation
- Audit logging for course creation events

### Performance Considerations:
- Async/await pattern throughout
- Efficient database queries
- Proper indexing recommendations
- Caching strategy for department/instructor lookups

Implement this functionality following the mvc architecture principles, SOLID principles, and the repository pattern established in the application. Ensure all code follows the .NET coding standards and includes proper error handling, logging, and documentation.
