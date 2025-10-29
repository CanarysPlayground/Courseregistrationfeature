using System.ComponentModel.DataAnnotations;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.DTOs
{
    // Registration DTOs
    public class CreateRegistrationDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateRegistrationDto
    {
        [Required]
        public RegistrationStatus Status { get; set; }

        public Grade? Grade { get; set; }

        public DateTime? CompletionDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class RegistrationResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public RegistrationStatus Status { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public Grade? Grade { get; set; }
        public string GradeDescription { get; set; } = string.Empty;
        public DateTime? CompletionDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
    }

    // User DTOs
    public class CreateUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        public int? StudentId { get; set; }
        public int? InstructorId { get; set; }
    }

    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        public int? StudentId { get; set; }
        public int? InstructorId { get; set; }
    }

    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RoleDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public int? StudentId { get; set; }
        public int? InstructorId { get; set; }
        public string? StudentName { get; set; }
        public string? InstructorName { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}