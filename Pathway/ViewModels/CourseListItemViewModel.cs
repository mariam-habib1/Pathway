namespace Pathway.ViewModels
{
    public class CourseListItemViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
