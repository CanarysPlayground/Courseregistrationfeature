using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        // Navigation property
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }
}