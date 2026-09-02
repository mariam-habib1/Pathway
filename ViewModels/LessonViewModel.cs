using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public int Order { get; set; }
    }

    public class LessonsPageViewModel
    {
        public int SectionId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public bool CanManage { get; set; } // true for the owning instructor or an admin
        public List<LessonViewModel> Lessons { get; set; } = new();
    }

    public class LessonDetailsViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public int Order { get; set; }
        public int SectionId { get; set; }
        public string SectionTitle { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
    }

    public class LessonFormViewModel
    {
        public int LessonId { get; set; }

        public int SectionId { get; set; }

        [Required(ErrorMessage = "Lesson title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Lesson title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
        public string? Content { get; set; }

        [Url(ErrorMessage = "Please enter a valid video URL")]
        [StringLength(500, ErrorMessage = "Video URL cannot exceed 500 characters")]
        public string? VideoUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0")]
        public int Order { get; set; } = 1;
    }
}
