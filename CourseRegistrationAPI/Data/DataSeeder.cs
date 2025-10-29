using CourseRegistrationAPI.Models;
using CourseRegistrationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistrationAPI.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(CourseRegistrationContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists
            if (await context.Departments.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Seed Departments
            var departments = new List<Department>
            {
                new Department
                {
                    Name = "Computer Science",
                    Description = "Department of Computer Science and Engineering",
                    HeadOfDepartment = "Dr. Sarah Johnson"
                },
                new Department
                {
                    Name = "Mathematics",
                    Description = "Department of Mathematics and Statistics",
                    HeadOfDepartment = "Dr. Michael Chen"
                },
                new Department
                {
                    Name = "Physics",
                    Description = "Department of Physics and Astronomy",
                    HeadOfDepartment = "Dr. Emily Rodriguez"
                },
                new Department
                {
                    Name = "Business Administration",
                    Description = "School of Business Administration",
                    HeadOfDepartment = "Dr. Robert Davis"
                }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();

            // Seed Instructors
            var instructors = new List<Instructor>
            {
                new Instructor
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john.smith@university.edu",
                    DepartmentId = departments[0].Id, // Computer Science
                    HireDate = DateTime.UtcNow.AddYears(-5),
                    Phone = "555-0101",
                    Bio = "Professor of Computer Science with expertise in software engineering and database systems."
                },
                new Instructor
                {
                    FirstName = "Lisa",
                    LastName = "Anderson",
                    Email = "lisa.anderson@university.edu",
                    DepartmentId = departments[0].Id, // Computer Science
                    HireDate = DateTime.UtcNow.AddYears(-3),
                    Phone = "555-0102",
                    Bio = "Associate Professor specializing in artificial intelligence and machine learning."
                },
                new Instructor
                {
                    FirstName = "David",
                    LastName = "Wilson",
                    Email = "david.wilson@university.edu",
                    DepartmentId = departments[1].Id, // Mathematics
                    HireDate = DateTime.UtcNow.AddYears(-7),
                    Phone = "555-0103",
                    Bio = "Professor of Mathematics with research focus on calculus and differential equations."
                },
                new Instructor
                {
                    FirstName = "Maria",
                    LastName = "Garcia",
                    Email = "maria.garcia@university.edu",
                    DepartmentId = departments[2].Id, // Physics
                    HireDate = DateTime.UtcNow.AddYears(-4),
                    Phone = "555-0104",
                    Bio = "Assistant Professor of Physics specializing in quantum mechanics and thermodynamics."
                },
                new Instructor
                {
                    FirstName = "James",
                    LastName = "Brown",
                    Email = "james.brown@university.edu",
                    DepartmentId = departments[3].Id, // Business Administration
                    HireDate = DateTime.UtcNow.AddYears(-6),
                    Phone = "555-0105",
                    Bio = "Professor of Business Administration with expertise in management and marketing."
                }
            };

            await context.Instructors.AddRangeAsync(instructors);
            await context.SaveChangesAsync();

            // Seed Students
            var students = new List<Student>
            {
                new Student
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@student.edu",
                    DateOfBirth = new DateTime(2000, 3, 15),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-18),
                    Phone = "555-1001",
                    Address = "123 Main St, College Town, ST 12345"
                },
                new Student
                {
                    FirstName = "Bob",
                    LastName = "Williams",
                    Email = "bob.williams@student.edu",
                    DateOfBirth = new DateTime(1999, 7, 22),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-24),
                    Phone = "555-1002",
                    Address = "456 Oak Ave, College Town, ST 12345"
                },
                new Student
                {
                    FirstName = "Carol",
                    LastName = "Davis",
                    Email = "carol.davis@student.edu",
                    DateOfBirth = new DateTime(2001, 11, 8),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-12),
                    Phone = "555-1003",
                    Address = "789 Pine St, College Town, ST 12345"
                },
                new Student
                {
                    FirstName = "Daniel",
                    LastName = "Miller",
                    Email = "daniel.miller@student.edu",
                    DateOfBirth = new DateTime(2000, 5, 30),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-20),
                    Phone = "555-1004",
                    Address = "321 Elm St, College Town, ST 12345"
                },
                new Student
                {
                    FirstName = "Eva",
                    LastName = "Thompson",
                    Email = "eva.thompson@student.edu",
                    DateOfBirth = new DateTime(1998, 12, 3),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-30),
                    Phone = "555-1005",
                    Address = "654 Maple Dr, College Town, ST 12345"
                },
                new Student
                {
                    FirstName = "Frank",
                    LastName = "White",
                    Email = "frank.white@student.edu",
                    DateOfBirth = new DateTime(2001, 9, 17),
                    EnrollmentDate = DateTime.UtcNow.AddMonths(-8),
                    Phone = "555-1006",
                    Address = "987 Cedar Ln, College Town, ST 12345"
                }
            };

            await context.Students.AddRangeAsync(students);
            await context.SaveChangesAsync();

            // Seed Courses
            var courses = new List<Course>
            {
                new Course
                {
                    Title = "Introduction to Programming",
                    Description = "Learn the fundamentals of programming using C# and .NET framework. Topics include variables, control structures, functions, and object-oriented programming basics.",
                    Credits = 3,
                    MaxCapacity = 30,
                    StartDate = DateTime.UtcNow.AddDays(30),
                    EndDate = DateTime.UtcNow.AddDays(150),
                    InstructorId = instructors[0].Id, // John Smith
                    DepartmentId = departments[0].Id, // Computer Science
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Database Systems",
                    Description = "Comprehensive study of database design, implementation, and management. Covers SQL, normalization, transactions, and database administration.",
                    Credits = 4,
                    MaxCapacity = 25,
                    StartDate = DateTime.UtcNow.AddDays(45),
                    EndDate = DateTime.UtcNow.AddDays(165),
                    InstructorId = instructors[0].Id, // John Smith
                    DepartmentId = departments[0].Id, // Computer Science
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Machine Learning Fundamentals",
                    Description = "Introduction to machine learning algorithms and techniques. Covers supervised and unsupervised learning, neural networks, and practical applications.",
                    Credits = 3,
                    MaxCapacity = 20,
                    StartDate = DateTime.UtcNow.AddDays(60),
                    EndDate = DateTime.UtcNow.AddDays(180),
                    InstructorId = instructors[1].Id, // Lisa Anderson
                    DepartmentId = departments[0].Id, // Computer Science
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Calculus I",
                    Description = "First course in calculus covering limits, derivatives, and applications. Essential foundation for advanced mathematics and engineering.",
                    Credits = 4,
                    MaxCapacity = 40,
                    StartDate = DateTime.UtcNow.AddDays(35),
                    EndDate = DateTime.UtcNow.AddDays(155),
                    InstructorId = instructors[2].Id, // David Wilson
                    DepartmentId = departments[1].Id, // Mathematics
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Statistics and Probability",
                    Description = "Introduction to statistical methods and probability theory. Covers descriptive statistics, hypothesis testing, and data analysis techniques.",
                    Credits = 3,
                    MaxCapacity = 35,
                    StartDate = DateTime.UtcNow.AddDays(50),
                    EndDate = DateTime.UtcNow.AddDays(170),
                    InstructorId = instructors[2].Id, // David Wilson
                    DepartmentId = departments[1].Id, // Mathematics
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "General Physics I",
                    Description = "Introduction to classical mechanics, including kinematics, dynamics, energy, and momentum. Laboratory work included.",
                    Credits = 4,
                    MaxCapacity = 30,
                    StartDate = DateTime.UtcNow.AddDays(40),
                    EndDate = DateTime.UtcNow.AddDays(160),
                    InstructorId = instructors[3].Id, // Maria Garcia
                    DepartmentId = departments[2].Id, // Physics
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Business Management",
                    Description = "Fundamentals of business management including planning, organizing, leading, and controlling. Case studies and practical applications.",
                    Credits = 3,
                    MaxCapacity = 50,
                    StartDate = DateTime.UtcNow.AddDays(25),
                    EndDate = DateTime.UtcNow.AddDays(145),
                    InstructorId = instructors[4].Id, // James Brown
                    DepartmentId = departments[3].Id, // Business Administration
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Course
                {
                    Title = "Marketing Principles",
                    Description = "Introduction to marketing concepts, consumer behavior, market research, and marketing strategies for modern businesses.",
                    Credits = 3,
                    MaxCapacity = 45,
                    StartDate = DateTime.UtcNow.AddDays(55),
                    EndDate = DateTime.UtcNow.AddDays(175),
                    InstructorId = instructors[4].Id, // James Brown
                    DepartmentId = departments[3].Id, // Business Administration
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsActive = true
                }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            // Seed Users
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@university.edu",
                    PasswordHash = "admin123", // In production, this should be hashed
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    FirstName = "System",
                    LastName = "Administrator",
                    Phone = "555-0000"
                },
                new User
                {
                    Username = "alice.student",
                    Email = "alice.johnson@student.edu",
                    PasswordHash = "student123", // In production, this should be hashed
                    Role = UserRole.Student,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Phone = "555-1001",
                    StudentId = students[0].Id
                },
                new User
                {
                    Username = "bob.student",
                    Email = "bob.williams@student.edu",
                    PasswordHash = "student123", // In production, this should be hashed
                    Role = UserRole.Student,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    FirstName = "Bob",
                    LastName = "Williams",
                    Phone = "555-1002",
                    StudentId = students[1].Id
                },
                new User
                {
                    Username = "john.instructor",
                    Email = "john.smith@university.edu",
                    PasswordHash = "instructor123", // In production, this should be hashed
                    Role = UserRole.Instructor,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    FirstName = "John",
                    LastName = "Smith",
                    Phone = "555-0101",
                    InstructorId = instructors[0].Id
                },
                new User
                {
                    Username = "lisa.instructor",
                    Email = "lisa.anderson@university.edu",
                    PasswordHash = "instructor123", // In production, this should be hashed
                    Role = UserRole.Instructor,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    FirstName = "Lisa",
                    LastName = "Anderson",
                    Phone = "555-0102",
                    InstructorId = instructors[1].Id
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Seed Sample Registrations
            var registrations = new List<Registration>
            {
                new Registration
                {
                    StudentId = students[0].Id, // Alice Johnson
                    CourseId = courses[0].Id, // Introduction to Programming
                    RegistrationDate = DateTime.UtcNow.AddDays(-10),
                    Status = RegistrationStatus.Enrolled,
                    Notes = "Student is doing well in the course"
                },
                new Registration
                {
                    StudentId = students[0].Id, // Alice Johnson
                    CourseId = courses[3].Id, // Calculus I
                    RegistrationDate = DateTime.UtcNow.AddDays(-8),
                    Status = RegistrationStatus.Enrolled,
                    Notes = "Strong mathematical background"
                },
                new Registration
                {
                    StudentId = students[1].Id, // Bob Williams
                    CourseId = courses[0].Id, // Introduction to Programming
                    RegistrationDate = DateTime.UtcNow.AddDays(-12),
                    Status = RegistrationStatus.Enrolled,
                    Notes = "Needs additional support with programming concepts"
                },
                new Registration
                {
                    StudentId = students[1].Id, // Bob Williams
                    CourseId = courses[6].Id, // Business Management
                    RegistrationDate = DateTime.UtcNow.AddDays(-5),
                    Status = RegistrationStatus.Enrolled
                },
                new Registration
                {
                    StudentId = students[2].Id, // Carol Davis
                    CourseId = courses[2].Id, // Machine Learning Fundamentals
                    RegistrationDate = DateTime.UtcNow.AddDays(-7),
                    Status = RegistrationStatus.Enrolled,
                    Notes = "Excellent student with strong technical skills"
                },
                new Registration
                {
                    StudentId = students[3].Id, // Daniel Miller
                    CourseId = courses[5].Id, // General Physics I
                    RegistrationDate = DateTime.UtcNow.AddDays(-15),
                    Status = RegistrationStatus.Enrolled
                },
                new Registration
                {
                    StudentId = students[4].Id, // Eva Thompson
                    CourseId = courses[7].Id, // Marketing Principles
                    RegistrationDate = DateTime.UtcNow.AddDays(-3),
                    Status = RegistrationStatus.Enrolled,
                    Notes = "Transfer student with prior business experience"
                }
            };

            await context.Registrations.AddRangeAsync(registrations);
            await context.SaveChangesAsync();

            Console.WriteLine("Sample data seeded successfully!");
        }
    }
}