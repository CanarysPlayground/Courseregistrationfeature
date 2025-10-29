using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStudentAsync(int studentId)
        {
            return await _dbSet
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Department)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public async Task<Registration?> GetRegistrationByStudentAndCourseAsync(int studentId, int courseId)
        {
            return await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<bool> IsStudentRegisteredForCourseAsync(int studentId, int courseId)
        {
            return await _dbSet
                .AnyAsync(r => r.StudentId == studentId && r.CourseId == courseId);
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsByStatusAsync(RegistrationStatus status)
        {
            return await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public override async Task<Registration?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public override async Task<IEnumerable<Registration>> GetAllAsync()
        {
            return await _dbSet
                .Include(r => r.Student)
                .Include(r => r.Course)
                    .ThenInclude(c => c.Instructor)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CourseRegistrationContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.Student)
                .Include(u => u.Instructor)
                    .ThenInclude(i => i!.Department)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Student)
                .Include(u => u.Instructor)
                    .ThenInclude(i => i!.Department)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _dbSet
                .Include(u => u.Student)
                .Include(u => u.Instructor)
                    .ThenInclude(i => i!.Department)
                .Where(u => u.Role == role && u.IsActive)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
            
            if (user == null)
                return false;

            // In a real application, you would hash the password and compare
            // This is simplified for demonstration purposes
            return user.PasswordHash == password;
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Student)
                .Include(u => u.Instructor)
                    .ThenInclude(i => i!.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbSet
                .Include(u => u.Student)
                .Include(u => u.Instructor)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
    }
}