using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Instructor?> GetByEmailAsync(string email);
        Task<IEnumerable<Instructor>> GetInstructorsByDepartmentAsync(int departmentId);
        Task<IEnumerable<Course>> GetInstructorCoursesAsync(int instructorId);
        Task<bool> IsAvailableForScheduleAsync(int instructorId, DateTime startDate, DateTime endDate);
    }

    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> GetByNameAsync(string name);
        Task<IEnumerable<Instructor>> GetDepartmentInstructorsAsync(int departmentId);
        Task<IEnumerable<Course>> GetDepartmentCoursesAsync(int departmentId);
    }

    public interface IRegistrationRepository : IRepository<Registration>
    {
        Task<IEnumerable<Registration>> GetRegistrationsByStudentAsync(int studentId);
        Task<IEnumerable<Registration>> GetRegistrationsByCourseAsync(int courseId);
        Task<Registration?> GetRegistrationByStudentAndCourseAsync(int studentId, int courseId);
        Task<bool> IsStudentRegisteredForCourseAsync(int studentId, int courseId);
        Task<IEnumerable<Registration>> GetRegistrationsByStatusAsync(RegistrationStatus status);
    }

    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> ValidateUserAsync(string username, string password);
    }
}