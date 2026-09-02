namespace Pathway.Services
{
    public class CategoryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? CategoryId { get; set; }

        public static CategoryResult Ok(string message = "", int? categoryId = null)
            => new CategoryResult { Success = true, Message = message, CategoryId = categoryId };

        public static CategoryResult Fail(string message)
            => new CategoryResult { Success = false, Message = message };
    }
}
