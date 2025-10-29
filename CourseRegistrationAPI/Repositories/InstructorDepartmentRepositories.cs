using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<Instructor?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .Where(i => i.DepartmentId == departmentId)
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId)
        {
            return await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Registrations)
                .Where(c => c.InstructorId == instructorId)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<bool> IsAvailableForScheduleAsync(int instructorId, DateTime startDate, DateTime endDate)
        {
            return !await _context.Courses
                .AnyAsync(c => c.InstructorId == instructorId &&
                              c.IsActive &&
                              ((c.StartDate <= endDate && c.EndDate >= startDate)));
        }

        public override async Task<Instructor?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public override async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.Department)
                .Include(i => i.Courses)
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstName)
                .ToListAsync();
        }
    }

    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(d => d.Instructors)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.Name == name);
        }

        public async Task<IEnumerable<Instructor>> GetDepartmentInstructorsAsync(int departmentId)
        {
            return await _context.Instructors
                .Include(i => i.Courses)
                .Where(i => i.DepartmentId == departmentId)
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetDepartmentCoursesAsync(int departmentId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Registrations)
                .Where(c => c.DepartmentId == departmentId)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public override async Task<Department?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Instructors)
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public override async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.Instructors)
                .Include(d => d.Courses)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }
    }
}