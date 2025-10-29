using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int instructorId);
        Task<IEnumerable<Course>> GetCoursesByDepartmentAsync(int departmentId);
        Task<IEnumerable<Course>> GetCoursesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Course>> GetAvailableCoursesAsync();
        Task<bool> HasScheduleConflictAsync(int instructorId, DateTime startDate, DateTime endDate, int? excludeCourseId = null);
        Task<IEnumerable<Registration>> GetCourseRegistrationsAsync(int courseId);
    }
}