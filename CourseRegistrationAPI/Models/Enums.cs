namespace CourseRegistrationAPI.Models
{
    public enum RegistrationStatus
    {
        Pending = 0,
        Enrolled = 1,
        Completed = 2,
        Dropped = 3,
        Cancelled = 4
    }

    public enum Grade
    {
        None = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        F = 5,
        Incomplete = 6,
        Withdrawn = 7
    }
}