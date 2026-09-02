namespace Pathway.ViewModels
{
    public class CourseSectionViewModel
    {
        public int SectionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int Order { get; set; }
        public int LessonCount { get; set; }
    }

    public class CourseSectionsPageViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public List<CourseSectionViewModel> Sections { get; set; } = new();
    }
}
