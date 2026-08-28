using Pathway.Models;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult Ok() => new() { Success = true };
        public static ServiceResult Fail(string message) => new() { Success = false, ErrorMessage = message };
    }

    public interface ICourseService
    {
        Task<List<CourseListItemViewModel>> GetAllCoursesAsync();
        Task<List<CourseListItemViewModel>> GetInstructorCoursesAsync(int instructorId);
        Task<CourseDetailsViewModel?> GetCourseDetailsAsync(int courseId);
        Task<List<CourseListItemViewModel>> SearchCoursesAsync(string keyword);
        Task<List<CourseListItemViewModel>> GetCoursesByCategoryAsync(int categoryId);
        Task<ServiceResult> CreateCourseAsync(CourseCreateViewModel model, int instructorId);
        Task<ServiceResult> UpdateCourseAsync(CourseEditViewModel model, int currentUserId, bool isAdmin);
        Task<ServiceResult> DeleteCourseAsync(int courseId, int currentUserId, bool isAdmin);
        Task<Course?> GetCourseForEditAsync(int courseId, int currentUserId, bool isAdmin);
    }
}
