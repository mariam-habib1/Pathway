namespace Pathway.ViewModels
{
    public class CategoryStat
    {
        public string CategoryName { get; set; } = string.Empty;
        public int CourseCount { get; set; }
    }

    public class InstructorStat
    {
        public string InstructorName { get; set; } = string.Empty;
        public int CourseCount { get; set; }
    }

    public class AdminStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalCourses { get; set; }
        public int TotalCategories { get; set; }
        public int TotalEnrollments { get; set; }
        public decimal AverageCoursePrice { get; set; }
        public double AverageProgress { get; set; }

        public List<CategoryStat> CoursesByCategory { get; set; } = new();
        public List<InstructorStat> TopInstructors { get; set; } = new();
    }
}
