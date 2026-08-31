namespace Pathway.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int EnrollmentCount { get; set; }
        public List<SectionSummaryVM> Sections { get; set; } = new();
    }

    public class SectionSummaryVM
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public int LessonCount { get; set; }
    }
}
