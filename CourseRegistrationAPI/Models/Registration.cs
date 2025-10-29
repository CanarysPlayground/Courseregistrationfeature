using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseRegistrationAPI.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        public Grade? Grade { get; set; }

        public DateTime? CompletionDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;

        [NotMapped]
        public bool IsActive => Status == RegistrationStatus.Enrolled || Status == RegistrationStatus.Pending;

        [NotMapped]
        public string StatusDescription => Status.ToString();

        [NotMapped]
        public string GradeDescription => Grade?.ToString() ?? "Not Graded";
    }
}