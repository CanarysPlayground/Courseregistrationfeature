using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationAPI.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? HeadOfDepartment { get; set; }

        // Navigation properties
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
}