using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllWithCourseCountAsync();
        Task<Category?> GetByIdAsync(int categoryId);
        Task<Category?> GetByIdWithCoursesAsync(int categoryId);
        Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null);
        Task<bool> HasCoursesAsync(int categoryId);
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
        Task<int> SaveChangesAsync();
    }
}
