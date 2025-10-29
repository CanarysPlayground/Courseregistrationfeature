---
mode: beastmode
---

# Unit Test Prompts for Add Course Feature

## Overview
Create comprehensive unit tests for the "Add Course" functionality in the Course Registration API. Focus on testing all layers of the clean architecture implementation, ensuring proper validation, business rules enforcement, and error handling.

## Test Categories and Prompts

### 1. DTO Validation Tests

#### CreateCourseDtoValidator Tests
Create unit tests to verify the FluentValidation rules for `CreateCourseDto`:

**Title Validation Tests:**
- Test that title is required (null, empty, whitespace should fail)
- Test title length validation (minimum 5 characters, maximum 200 characters)
- Test title format validation (only alphanumeric, spaces, and hyphens allowed)
- Test title with special characters (should fail validation)
- Test edge cases: exactly 5 characters, exactly 200 characters

**Description Validation Tests:**
- Test that description is required
- Test description length validation (minimum 10 characters, maximum 1000 characters)
- Test description with various content types (text, numbers, special characters)
- Test edge cases: exactly 10 characters, exactly 1000 characters

**Credits Validation Tests:**
- Test credits range validation (must be between 1-6)
- Test invalid credits values (0, 7, negative numbers)
- Test boundary values (1 and 6 should pass)

**MaxCapacity Validation Tests:**
- Test capacity range validation (must be between 1-500)
- Test invalid capacity values (0, 501, negative numbers)
- Test boundary values (1 and 500 should pass)

**Date Validation Tests:**
- Test StartDate must be at least 7 days in the future
- Test EndDate must be after StartDate
- Test minimum course duration (30 days)
- Test maximum course duration (if applicable)
- Test past dates (should fail)
- Test same start and end dates (should fail)

**Foreign Key Validation Tests:**
- Test InstructorId is required and must be positive
- Test DepartmentId is required and must be positive
- Test zero or negative foreign key values (should fail)

**Custom Business Rule Validation Tests:**
- Test instructor must belong to specified department
- Test instructor availability (no schedule conflicts)
- Test duplicate course detection (same title, instructor, overlapping dates)

### 2. Service Layer Tests (CourseService)

#### Course Creation Success Scenarios
Create tests for successful course creation:

**Valid Course Creation:**
- Test creating a course with all valid data
- Test course creation with minimum valid values
- Test course creation with maximum valid values
- Verify that CreatedDate and ModifiedDate are set automatically
- Verify that IsActive is set to true by default
- Test that course code is generated automatically

**Repository Interaction Tests:**
- Test that the service calls the repository to check for duplicates
- Test that the service calls the repository to check instructor availability
- Test that the service calls the repository to save the new course
- Test that the service returns the correct CourseResponseDto

#### Course Creation Failure Scenarios
Create tests for business rule violations:

**Duplicate Course Detection:**
- Test rejection when course with same title and instructor exists with overlapping dates
- Test acceptance when course with same title but different instructor
- Test acceptance when course with same instructor but non-overlapping dates

**Instructor Schedule Conflicts:**
- Test rejection when instructor has overlapping course schedules
- Test acceptance when instructor has non-overlapping schedules
- Test edge cases with courses that end/start on the same day

**Department Validation:**
- Test rejection when instructor doesn't belong to specified department
- Test acceptance when instructor belongs to specified department

**Exception Handling Tests:**
- Test handling of `CourseAlreadyExistsException`
- Test handling of `InstructorScheduleConflictException`
- Test handling of `DepartmentCapacityExceededException`
- Test handling of `InstructorDepartmentMismatchException`
- Test handling of repository exceptions
- Test handling of database connection failures

### 3. Repository Layer Tests

#### ICourseRepository Implementation Tests
Create tests for custom repository methods:

**ExistsByTitleAndInstructorAsync Tests:**
- Test returns true when course exists with same title and instructor
- Test returns false when no matching course exists
- Test case-insensitive title comparison
- Test with null parameters (should handle gracefully)

**HasScheduleConflictAsync Tests:**
- Test returns true when instructor has overlapping schedules
- Test returns false when no schedule conflicts exist
- Test edge cases (courses ending/starting on same day)
- Test with multiple existing courses for same instructor

**GetCoursesByInstructorAndPeriodAsync Tests:**
- Test retrieves all courses for instructor in specified period
- Test returns empty collection when no courses found
- Test date range filtering accuracy
- Test with overlapping and non-overlapping periods

**Standard Repository Method Tests:**
- Test CreateAsync adds course to database
- Test GetByIdAsync retrieves correct course
- Test GetAllAsync retrieves all active courses
- Test UpdateAsync modifies existing course
- Test DeleteAsync (soft delete) marks course as inactive

### 4. Controller Layer Tests (CoursesController)

#### HTTP POST /api/v1/courses Tests
Create tests for the API endpoint:

**Successful Course Creation (201 Created):**
- Test valid course creation returns 201 status
- Test response includes Location header with course URL
- Test response body contains created course data
- Test AutoMapper correctly maps domain model to response DTO

**Validation Failures (400 Bad Request):**
- Test invalid model state returns 400 status
- Test validation errors are included in response
- Test response format matches API standards
- Test multiple validation errors are returned together

**Business Rule Violations (409 Conflict):**
- Test duplicate course returns 409 status
- Test schedule conflict returns 409 status
- Test appropriate error messages in response

**Authorization Tests:**
- Test unauthorized requests return 401 status
- Test insufficient permissions return 403 status
- Test only Admin and Instructor roles can create courses
- Test Student role cannot create courses

**Exception Handling Tests:**
- Test unhandled exceptions return 500 status
- Test error responses don't expose sensitive information
- Test correlation IDs are included in error responses

### 5. Integration Tests

#### Full Course Creation Flow Tests
Create end-to-end integration tests:

**Complete Success Flow:**
- Test creating a course from HTTP request to database persistence
- Test all middleware executes correctly (authentication, validation, logging)
- Test database transaction handling
- Test audit logging is created

**Database Integration Tests:**
- Test with real database using TestContainers
- Test Entity Framework mappings work correctly
- Test foreign key constraints are enforced
- Test database indexes improve query performance

**Authentication Integration Tests:**
- Test JWT token validation works correctly
- Test role-based authorization functions properly
- Test token expiration handling

### 6. Performance and Load Tests

#### Performance Test Scenarios
Create tests to verify performance requirements:

**Response Time Tests:**
- Test course creation completes within acceptable time limits (e.g., < 500ms)
- Test validation performance with large datasets
- Test database query performance

**Concurrent Request Tests:**
- Test multiple simultaneous course creation requests
- Test race condition handling
- Test database locking behavior

### 7. Edge Case and Error Boundary Tests

#### Edge Case Scenarios
Create tests for unusual but valid scenarios:

**Boundary Value Tests:**
- Test with exactly minimum/maximum allowed values
- Test date boundaries (leap years, month boundaries)
- Test string length boundaries

**Null and Empty Value Tests:**
- Test handling of null request bodies
- Test empty string values
- Test whitespace-only values

**Malformed Request Tests:**
- Test invalid JSON in request body
- Test missing required headers
- Test unsupported content types

### 8. Security Tests

#### Security Validation Tests
Create tests to verify security measures:

**Input Sanitization Tests:**
- Test SQL injection attempts in course data
- Test XSS attempts in course descriptions
- Test malicious file paths in course titles

**Authorization Security Tests:**
- Test privilege escalation attempts
- Test accessing other users' data
- Test bypassing role restrictions

## Test Implementation Guidelines

### Test Structure Standards
- Use AAA pattern (Arrange, Act, Assert) for all tests
- Create descriptive test method names that clearly indicate the scenario
- Use test categories and traits for test organization
- Implement proper test data builders and factories

### Mocking Strategy
- Mock all external dependencies (repositories, services, HTTP context)
- Use Moq or NSubstitute for creating test doubles
- Verify method calls and parameter values in mocks
- Test both positive and negative scenarios for each mock

### Test Data Management
- Create reusable test data builders for DTOs and entities
- Use realistic test data that represents actual usage patterns
- Implement test data cleanup strategies
- Use parameterized tests for testing multiple similar scenarios

### Assertion Strategies
- Use FluentAssertions for readable test assertions
- Verify both success and failure outcomes
- Assert on specific error messages and types
- Validate response structure and content

### Test Coverage Goals
- Aim for minimum 80% code coverage for the Course feature
- Ensure all business logic paths are tested
- Cover both happy path and error scenarios
- Include boundary condition testing

## Test Organization Structure

```
Tests/
├── Unit/
│   ├── Validators/
│   │   └── CreateCourseDtoValidatorTests.cs
│   ├── Services/
│   │   └── CourseServiceTests.cs
│   ├── Repositories/
│   │   └── CourseRepositoryTests.cs
│   └── Controllers/
│       └── CoursesControllerTests.cs
├── Integration/
│   ├── CourseCreationIntegrationTests.cs
│   └── DatabaseIntegrationTests.cs
└── TestUtilities/
    ├── TestDataBuilders/
    ├── MockFactories/
    └── TestFixtures/
```

Each test class should focus on a single component and include comprehensive test coverage for both success and failure scenarios.

