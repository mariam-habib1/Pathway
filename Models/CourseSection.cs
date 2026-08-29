using System.ComponentModel.DataAnnotations;

namespace Pathway.Models
{
    public class CourseSection
    {
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Section title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Section title must be between 2 and 200 characters")]
        public string Title { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Order must be greater than 0")]
        public int Order { get; set; }

        public Course Course { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
