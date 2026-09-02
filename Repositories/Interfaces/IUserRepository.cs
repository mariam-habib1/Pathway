using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByIdWithDetailsAsync(int userId);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<bool> HasCoursesAsync(int instructorId);
        Task<bool> HasEnrollmentsAsync(int studentId);
        void Update(User user);
        void Delete(User user);
        Task<int> SaveChangesAsync();
    }
}
