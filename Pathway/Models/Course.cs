using System.ComponentModel.DataAnnotations;

namespace Pathway.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 200 characters")]
        public string Title { get; set; }

        [StringLength(2000,
            ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000,
            ErrorMessage = "Price must be between 0 and 100000")]
        public decimal Price { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime CreatedAt { get; set; }

        public User Instructor { get; set; }

        public Category Category { get; set; }

        public ICollection<CourseSection> Sections { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
