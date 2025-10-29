using System.ComponentModel.DataAnnotations;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.DTOs
{
    // Instructor DTOs
    public class CreateInstructorDto
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
        public int DepartmentId { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }
    }

    public class UpdateInstructorDto
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
        public int DepartmentId { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }
    }

    public class InstructorResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public int YearsOfService { get; set; }
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public int CourseCount { get; set; }
    }

    // Department DTOs
    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? HeadOfDepartment { get; set; }
    }

    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? HeadOfDepartment { get; set; }
    }

    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? HeadOfDepartment { get; set; }
        public int InstructorCount { get; set; }
        public int CourseCount { get; set; }
    }
}