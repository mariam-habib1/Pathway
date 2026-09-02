using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface ICourseSectionService
    {
        Task<CourseSectionsPageViewModel?> GetSectionsForCourseAsync(int courseId, int requestingUserId, bool isAdmin);
        Task<CourseSectionFormViewModel?> GetForEditAsync(int sectionId, int requestingUserId, bool isAdmin);
        Task<SectionResult> CreateAsync(int courseId, int requestingUserId, bool isAdmin, CourseSectionFormViewModel model);
        Task<SectionResult> UpdateAsync(int sectionId, int requestingUserId, bool isAdmin, CourseSectionFormViewModel model);
        Task<SectionResult> DeleteAsync(int sectionId, int requestingUserId, bool isAdmin);
        Task<CourseContentBuilderViewModel?> GetContentBuilderAsync(int courseId, int requestingUserId, bool isAdmin);
        Task<SectionResult> SaveContentAsync(int courseId, int requestingUserId, bool isAdmin, CourseContentBuilderViewModel model);
    }
}
