namespace Pathway.Services
{
    public class EnrollmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? EnrollmentId { get; set; }

        public static EnrollmentResult Ok(string message = "", int? enrollmentId = null)
            => new EnrollmentResult { Success = true, Message = message, EnrollmentId = enrollmentId };

        public static EnrollmentResult Fail(string message)
            => new EnrollmentResult { Success = false, Message = message };
    }
}
