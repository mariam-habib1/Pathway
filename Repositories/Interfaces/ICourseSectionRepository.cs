using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface ICourseSectionRepository
    {
        Task<List<CourseSection>> GetByCourseIdAsync(int courseId);
        Task<CourseSection?> GetByIdAsync(int sectionId);
        Task<CourseSection?> GetByIdWithCourseAsync(int sectionId);
        Task AddAsync(CourseSection section);
        void Update(CourseSection section);
        void Delete(CourseSection section);
        Task<int> SaveChangesAsync();
    }
}
