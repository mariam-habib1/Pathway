using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonsPageViewModel?> GetLessonsForSectionAsync(int sectionId, int requestingUserId, bool isAdmin);
        Task<LessonDetailsViewModel?> GetLessonDetailsAsync(int lessonId, int requestingUserId, bool isAdmin);
        Task<LessonFormViewModel?> GetForEditAsync(int lessonId, int requestingUserId, bool isAdmin);
        Task<LessonResult> CreateAsync(int sectionId, int requestingUserId, bool isAdmin, LessonFormViewModel model);
        Task<LessonResult> UpdateAsync(int lessonId, int requestingUserId, bool isAdmin, LessonFormViewModel model);
        Task<LessonResult> DeleteAsync(int lessonId, int requestingUserId, bool isAdmin);
    }
}
