using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class CourseEditViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<SelectListItemVM>? Categories { get; set; }
    }
}
