namespace Pathway.Services
{
    public class CourseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? CourseId { get; set; }

        public static CourseResult Ok(string message = "", int? courseId = null)
            => new CourseResult { Success = true, Message = message, CourseId = courseId };

        public static CourseResult Fail(string message)
            => new CourseResult { Success = false, Message = message };
    }
}
