using CourseRegistrationAPI.Models;

namespace CourseRegistrationAPI.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByEmailAsync(string email);
        Task<IEnumerable<Student>> GetStudentsByRegistrationDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Registration>> GetStudentRegistrationsAsync(int studentId);
    }
}