namespace Pathway.ViewModels
{
    public class StudentDashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int InProgressCourses { get; set; }
        public double AverageProgress { get; set; }

        public List<EnrollmentViewModel> Enrollments { get; set; } = new();
    }
}