using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class CourseSectionFormViewModel
    {
        public int SectionId { get; set; }

        public int CourseId { get; set; }

        [Required(ErrorMessage = "Section title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Section title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Order must be greater than 0")]
        public int Order { get; set; } = 1;
    }
}
