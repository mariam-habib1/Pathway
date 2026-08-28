using Pathway.Models;

namespace Pathway.Data
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<Course?> GetByIdWithSectionsAsync(int id);
        Task<List<Course>> GetByInstructorAsync(int instructorId);
        Task<List<Course>> SearchAsync(string keyword);
        Task<List<Course>> GetByCategoryAsync(int categoryId);
        Task AddAsync(Course course);
        void Update(Course course);
        void Delete(Course course);
        Task<bool> SaveChangesAsync();
    }
}
