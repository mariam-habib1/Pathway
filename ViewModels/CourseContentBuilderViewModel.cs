namespace Pathway.ViewModels
{
    public class ContentBuilderLessonViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
    }

    public class ContentBuilderSectionViewModel
    {
        public string Title { get; set; } = string.Empty;
        public List<ContentBuilderLessonViewModel> Lessons { get; set; } = new();
    }

    public class CourseContentBuilderViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public List<ContentBuilderSectionViewModel> Sections { get; set; } = new();
    }
}