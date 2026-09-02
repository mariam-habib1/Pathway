using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseViewModel>> GetAllAsync();
        Task<CourseDetailsViewModel?> GetDetailsAsync(int courseId, int? currentUserId = null);
        Task<CourseFormViewModel> GetEmptyFormAsync();
        Task<CourseFormViewModel?> GetForEditAsync(int courseId, int requestingUserId, bool isAdmin);
        Task<CourseResult> CreateAsync(int instructorId, CourseFormViewModel model);
        Task<CourseResult> UpdateAsync(int courseId, int requestingUserId, bool isAdmin, CourseFormViewModel model);
        Task<CourseResult> DeleteAsync(int courseId, int requestingUserId, bool isAdmin);
        Task<List<CourseViewModel>> GetByInstructorAsync(int instructorId);
        Task<List<CourseViewModel>> SearchAsync(string keyword);
        Task<List<CourseViewModel>> GetByCategoryAsync(int categoryId);
    }
}
