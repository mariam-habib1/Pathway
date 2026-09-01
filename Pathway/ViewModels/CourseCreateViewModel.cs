using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class CourseCreateViewModel
    {
        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        // Populated by controller to fill the dropdown
        public List<SelectListItemVM>? Categories { get; set; }
    }

    public class SelectListItemVM
    {
        public int Value { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}