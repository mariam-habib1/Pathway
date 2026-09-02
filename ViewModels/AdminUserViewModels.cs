using System.ComponentModel.DataAnnotations;

namespace Pathway.ViewModels
{
    public class AdminUserListItemViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AdminUserCourseItem
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class AdminUserEnrollmentItem
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public int Progress { get; set; }
    }

    public class AdminUserDetailsViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Populated only when Role == "Instructor"
        public List<AdminUserCourseItem> Courses { get; set; } = new();

        // Populated only when Role == "Student"
        public List<AdminUserEnrollmentItem> Enrollments { get; set; } = new();
    }

    public class AdminUserEditViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;
    }
}
