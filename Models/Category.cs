using System.ComponentModel.DataAnnotations;

namespace Pathway.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Category name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
