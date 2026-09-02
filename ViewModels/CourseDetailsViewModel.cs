namespace Pathway.ViewModels
{
    public class CourseLessonSummaryViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
    }

    public class CourseSectionSummaryViewModel
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public int LessonCount { get; set; }
        public List<CourseLessonSummaryViewModel> Lessons { get; set; } = new();
    }

    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int EnrollmentCount { get; set; }
        public bool IsEnrolled { get; set; }
        public bool CanViewContent { get; set; }
        public List<CourseSectionSummaryViewModel> Sections { get; set; } = new();
    }
}
