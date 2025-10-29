using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .Where(c => c.InstructorId == instructorId)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .Where(c => c.DepartmentId == departmentId)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .Where(c => c.StartDate >= startDate && c.EndDate <= endDate)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAvailableCoursesAsync()
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .Where(c => c.IsActive && c.StartDate > DateTime.Now)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<bool> HasScheduleConflictAsync(int instructorId, DateTime startDate, DateTime endDate, int? excludeCourseId = null)
        {
            var query = _dbSet.Where(c => c.InstructorId == instructorId &&
                                         c.IsActive &&
                                         ((c.StartDate <= endDate && c.EndDate >= startDate)));

            if (excludeCourseId.HasValue)
            {
                query = query.Where(c => c.Id != excludeCourseId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Registration>> GetCourseRegistrationsAsync(int courseId)
        {
            return await _context.Registrations
                .Include(r => r.Student)
                .Include(r => r.Course)
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public override async Task<Course?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Instructor)
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }
    }
}