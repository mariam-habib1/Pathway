using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        Task<List<Lesson>> GetBySectionIdAsync(int sectionId);
        Task<Lesson?> GetByIdAsync(int lessonId);
        Task<Lesson?> GetByIdWithSectionAsync(int lessonId);
        Task AddAsync(Lesson lesson);
        void Update(Lesson lesson);
        void Delete(Lesson lesson);
        Task<int> SaveChangesAsync();
    }
}
