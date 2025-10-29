using System.ComponentModel.DataAnnotations;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.DTOs
{
    // Course DTOs
    public class CreateCourseDto
    {
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
    }

    public class UpdateCourseDto
    {
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

        public bool IsActive { get; set; } = true;
    }

    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int MaxCapacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int CurrentEnrollment { get; set; }
        public int AvailableSpots { get; set; }
        public bool IsFull { get; set; }
        public int DurationInDays { get; set; }
    }
}