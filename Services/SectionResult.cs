namespace Pathway.Services
{
    public class SectionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? SectionId { get; set; }
        public int? CourseId { get; set; }

        public static SectionResult Ok(string message, int? sectionId = null, int? courseId = null)
            => new SectionResult { Success = true, Message = message, SectionId = sectionId, CourseId = courseId };

        public static SectionResult Fail(string message)
            => new SectionResult { Success = false, Message = message };
    }
}
