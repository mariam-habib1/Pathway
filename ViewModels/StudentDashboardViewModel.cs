namespace Pathway.ViewModels
{
    public class StudentDashboardViewModel
    {
        public int TotalCourses { get; set; }

        public List<EnrollmentViewModel> Enrollments { get; set; } = new();
    }
}
