using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationAPI.DTOs
{
    // Student DTOs
    public class CreateStudentDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }

    public class UpdateStudentDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }
    }

    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int RegistrationCount { get; set; }
    }
}