namespace Pathway.ViewModels
{
    public class CategoryCourseViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string InstructorName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class CategoryDetailsViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<CategoryCourseViewModel> Courses { get; set; } = new();
    }
}
