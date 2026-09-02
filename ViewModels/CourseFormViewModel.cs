using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pathway.ViewModels
{
    public class CourseFormViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Thumbnail URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? ThumbnailUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please choose a category")]
        public int CategoryId { get; set; }

        public List<SelectListItem> CategoryOptions { get; set; } = new();
    }
}
