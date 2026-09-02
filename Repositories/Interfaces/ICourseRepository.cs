using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int courseId);
        Task<Course?> GetByIdWithDetailsAsync(int courseId);
        Task<List<Course>> GetByInstructorIdAsync(int instructorId);
        Task<List<Course>> SearchAsync(string keyword);
        Task<List<Course>> GetByCategoryIdAsync(int categoryId);
        Task AddAsync(Course course);
        void Update(Course course);
        void Delete(Course course);
        Task<int> SaveChangesAsync();
    }
}
