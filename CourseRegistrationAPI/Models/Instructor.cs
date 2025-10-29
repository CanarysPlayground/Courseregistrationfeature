using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationAPI.Models
{
    public class Instructor
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
        public int DepartmentId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        // Navigation properties
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;
        
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int YearsOfService => DateTime.Now.Year - HireDate.Year;
    }
}