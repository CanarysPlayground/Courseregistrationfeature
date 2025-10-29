---
mode: agent
---

# Certificate Generation System Implementation

## Overview
Implement a certificate generation system that allows users to generate and download completion certificates for courses they have successfully completed in the Course Registration System.

## Requirements

### 1. Backend API Requirements

#### Certificate Model
Create a new `Certificate.cs` model with the following properties:
- `Id` (int): Primary key
- `StudentId` (int): Foreign key to Student
- `CourseId` (int): Foreign key to Course  
- `IssueDate` (DateTime): Certificate generation date
- `CertificateNumber` (string): Unique certificate identifier
- `StudentName` (string): Full name of the student
- `CourseName` (string): Title of the completed course
- `CompletionDate` (DateTime): Course completion date
- `InstructorName` (string): Name of the course instructor
- `Grade` (string): Final grade achieved (optional)

#### Certificate Controller
Create `CertificateController.cs` with the following endpoints:

1. **GET /api/certificate/verify/{username}/{courseId}**
   - Verify if user has completed the specified course
   - Check registration status is "Completed"
   - Return eligibility status and course details

2. **POST /api/certificate/generate**
   - Request body: `{ "username": "string", "courseId": "int" }`
   - Validate user completion status
   - Generate unique certificate number (format: CERT-YYYY-MMDD-XXXXX)
   - Create certificate record in database
   - Return certificate data

3. **GET /api/certificate/download/{certificateId}**
   - Generate PDF certificate
   - Return PDF file for download

#### Certificate Repository
Create `ICertificateRepository.cs` and `CertificateRepository.cs`:
- Methods for CRUD operations on certificates
- Method to check if certificate already exists for user/course combination
- Method to get certificate by unique certificate number

#### PDF Generation Service
Create `CertificateService.cs`:
- Use a PDF library (iTextSharp or similar)
- Generate professional-looking certificate PDF
- Include institution logo/branding
- Implement certificate template with proper formatting

### 2. Frontend Implementation

#### Certificate Request Page (`/certificate`)
Create a user interface with:

**Page Layout:**
- Header: "Course Completion Certificate"
- Form section with:
  - Username input field (required)
  - Course selection dropdown (populated from API)
  - "Generate Certificate" button
- Responsive design following existing UI patterns

**Form Validation:**
- Username is required and must be valid
- Course selection is required
- Display appropriate error messages

**API Integration:**
- Fetch available courses from `/api/courses`
- Validate user eligibility before generation
- Handle loading states and error responses

#### Certificate Display Page (`/certificate/view/{certificateId}`)
Create certificate view with:

**Certificate Design:**
- Professional certificate layout
- Header: Institution name/logo
- Title: "Certificate of Completion"
- Body text: "{StudentName} has successfully completed {CourseName}"
- Course details: Credits, completion date, grade (if available)
- Instructor signature section: "Instructor: {InstructorName}"
- Certificate number and issue date
- Official seal/watermark

**Download Functionality:**
- "Download Certificate" button
- Generate and download PDF version
- Maintain certificate formatting in PDF

### 3. Database Updates

#### Migration Requirements
- Add Certificate table to database context
- Update `CourseRegistrationContext.cs` to include Certificate DbSet
- Create database migration for Certificate table
- Seed sample certificate data if needed

#### Relationship Updates
- Add navigation properties between Certificate and Student/Course models
- Ensure proper foreign key constraints

### 4. Security & Validation

#### Authentication & Authorization
- Implement user authentication to verify identity
- Only allow certificate generation for authenticated users
- Prevent unauthorized certificate access

#### Data Validation
- Validate user has "Completed" registration status
- Prevent duplicate certificate generation
- Validate course completion date is past
- Ensure grade meets minimum requirements (if applicable)

#### Certificate Security
- Generate unique, non-guessable certificate numbers
- Implement certificate verification system
- Add digital signatures or QR codes for verification

### 5. UI/UX Requirements

#### Design Standards
- Follow existing application design patterns
- Use consistent color scheme and typography
- Ensure mobile responsiveness
- Implement loading states and progress indicators

#### User Experience
- Clear error messages and validation feedback
- Intuitive navigation flow
- Print-friendly certificate layout
- Fast certificate generation and download

#### Accessibility
- WCAG 2.1 AA compliance
- Proper ARIA labels and keyboard navigation
- Screen reader compatibility
- High contrast mode support

### 6. Technical Specifications

#### Technology Stack
- Backend: ASP.NET Core Web API
- Frontend: React/Angular/Vue.js (based on existing stack)
- Database: Entity Framework Core
- PDF Generation: iTextSharp or PdfSharp
- Authentication: JWT or existing auth system

#### Performance Requirements
- Certificate generation should complete within 5 seconds
- PDF download should start immediately after generation
- Support concurrent certificate generation
- Implement caching for frequently accessed data

#### Error Handling
- Comprehensive error logging
- User-friendly error messages
- Graceful handling of network failures
- Retry mechanisms for PDF generation

### 7. Testing Requirements

#### Unit Tests
- Certificate controller endpoints
- Certificate service methods
- Repository operations
- PDF generation functionality

#### Integration Tests
- End-to-end certificate generation flow
- Database operations and relationships
- PDF download functionality
- API error scenarios

#### UI Testing
- Form validation and submission
- Certificate display rendering
- Download functionality
- Responsive design testing

### 8. Deployment Considerations

#### Configuration
- PDF template configuration
- Certificate number generation settings
- File storage configuration for PDFs
- Email notification settings (optional)

#### Scalability
- Implement PDF generation queuing for high volume
- Consider cloud storage for certificate files
- Optimize database queries for large datasets
- Implement caching strategies

### 9. Future Enhancements

#### Optional Features
- Email certificate delivery
- Bulk certificate generation for administrators
- Certificate verification portal
- Custom certificate templates per course
- Social media sharing integration
- Certificate expiration handling

## Acceptance Criteria

1. ✅ Users can enter username and select course to request certificate
2. ✅ System validates user has completed the selected course
3. ✅ Certificate displays correct student name, course title, and completion details
4. ✅ Certificate includes professional formatting with instructor signature
5. ✅ Users can download certificate as PDF
6. ✅ Certificate has unique identification number
7. ✅ System prevents duplicate certificate generation
8. ✅ All forms include proper validation and error handling
9. ✅ Certificate design is print-friendly and professional
10. ✅ System maintains certificate records for future reference

## Implementation Priority

### Phase 1 (Core Functionality)
- Certificate model and database schema
- Basic certificate generation API
- Simple certificate request form
- PDF generation with basic template

### Phase 2 (Enhanced Features)
- Professional certificate design
- User authentication integration
- Enhanced validation and security
- Comprehensive testing

### Phase 3 (Advanced Features)
- Certificate verification system
- Bulk operations for administrators
- Performance optimizations
- Additional export formats
