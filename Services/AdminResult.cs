namespace Pathway.Services
{
    public class AdminResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static AdminResult Ok(string message = "")
            => new AdminResult { Success = true, Message = message };

        public static AdminResult Fail(string message)
            => new AdminResult { Success = false, Message = message };
    }
}
