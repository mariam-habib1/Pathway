using System.ComponentModel.DataAnnotations;

namespace Pathway.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public DateTime EnrolledAt { get; set; }

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public int Progress { get; set; } = 0;

        public User Student { get; set; }

        public Course Course { get; set; }
    }
}
