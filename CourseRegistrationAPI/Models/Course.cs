using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationAPI.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 6)]
        public int Credits { get; set; }

        [Required]
        [Range(1, 500)]
        public int MaxCapacity { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("InstructorId")]
        public virtual Instructor Instructor { get; set; } = null!;

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } = null!;

        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        [NotMapped]
        public int CurrentEnrollment => Registrations?.Count(r => r.Status == RegistrationStatus.Enrolled) ?? 0;

        [NotMapped]
        public int AvailableSpots => MaxCapacity - CurrentEnrollment;

        [NotMapped]
        public bool IsFull => CurrentEnrollment >= MaxCapacity;

        [NotMapped]
        public int DurationInDays => (EndDate - StartDate).Days;
    }
}