using System.ComponentModel.DataAnnotations;

namespace Pathway.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        [Required(ErrorMessage = "Lesson title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Lesson title must be between 2 and 200 characters")]
        public string Title { get; set; }

        [StringLength(5000,
            ErrorMessage = "Content cannot exceed 5000 characters")]
        public string? Content { get; set; }

        [Url(ErrorMessage = "Please enter a valid video URL")]
        [StringLength(500, ErrorMessage = "Video URL cannot exceed 500 characters")]
        public string? VideoUrl { get; set; }

        [Required]
        public int SectionId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Order must be greater than 0")]
        public int Order { get; set; }

        public CourseSection Section { get; set; }
    }
}
