namespace Pathway.Services
{
    public class LessonResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? LessonId { get; set; }
        public int? SectionId { get; set; }

        public static LessonResult Ok(string message, int? lessonId = null, int? sectionId = null)
            => new LessonResult { Success = true, Message = message, LessonId = lessonId, SectionId = sectionId };

        public static LessonResult Fail(string message)
            => new LessonResult { Success = false, Message = message };
    }
}
