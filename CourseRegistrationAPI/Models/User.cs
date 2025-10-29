using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationAPI.Models
{
    public enum UserRole
    {
        Student = 0,
        Instructor = 1,
        Admin = 2
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Student;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        // Navigation properties - Optional references to Student/Instructor
        public int? StudentId { get; set; }
        public int? InstructorId { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Instructor? Instructor { get; set; }
    }
}