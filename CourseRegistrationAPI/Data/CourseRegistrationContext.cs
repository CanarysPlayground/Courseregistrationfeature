using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Data
{
    public class CourseRegistrationContext : DbContext
    {
        public CourseRegistrationContext(DbContextOptions<CourseRegistrationContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Address).HasMaxLength(200);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.HeadOfDepartment).HasMaxLength(100);
            });

            // Configure Instructor entity
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Bio).HasMaxLength(1000);

                // Foreign key relationship
                entity.HasOne(i => i.Department)
                      .WithMany(d => d.Instructors)
                      .HasForeignKey(i => i.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Credits).IsRequired();
                entity.Property(e => e.MaxCapacity).IsRequired();

                // Foreign key relationships
                entity.HasOne(c => c.Instructor)
                      .WithMany(i => i.Courses)
                      .HasForeignKey(c => c.InstructorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Department)
                      .WithMany(d => d.Courses)
                      .HasForeignKey(c => c.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Index for performance
                entity.HasIndex(e => new { e.InstructorId, e.StartDate, e.EndDate });
            });

            // Configure Registration entity
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).HasMaxLength(500);

                // Foreign key relationships
                entity.HasOne(r => r.Student)
                      .WithMany(s => s.Registrations)
                      .HasForeignKey(r => r.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Course)
                      .WithMany(c => c.Registrations)
                      .HasForeignKey(r => r.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique index to prevent duplicate registrations
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(15);

                // Optional relationships to Student and Instructor
                entity.HasOne(u => u.Student)
                      .WithMany()
                      .HasForeignKey(u => u.StudentId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(u => u.Instructor)
                      .WithMany()
                      .HasForeignKey(u => u.InstructorId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure enum conversions
            modelBuilder.Entity<Registration>()
                .Property(e => e.Status)
                .HasConversion<int>();

            modelBuilder.Entity<Registration>()
                .Property(e => e.Grade)
                .HasConversion<int>();

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<int>();
        }
    }
}