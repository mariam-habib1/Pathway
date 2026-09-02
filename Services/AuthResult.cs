namespace Pathway.Services
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public static AuthResult Ok(int userId, string name, string role, string message = "")
            => new AuthResult { Success = true, UserId = userId, Name = name, Role = role, Message = message };

        public static AuthResult Fail(string message)
            => new AuthResult { Success = false, Message = message };
    }
}
