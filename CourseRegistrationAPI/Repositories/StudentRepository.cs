using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Course)
                .FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task<IEnumerable<Student>> GetStudentsByRegistrationDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(s => s.EnrollmentDate >= startDate && s.EnrollmentDate <= endDate)
                .OrderBy(s => s.EnrollmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetStudentRegistrationsAsync(int studentId)
        {
            return await _context.Registrations
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Department)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public override async Task<Student?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Registrations)
                    .ThenInclude(r => r.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _dbSet
                .Include(s => s.Registrations)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }
    }
}