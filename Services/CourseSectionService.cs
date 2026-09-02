using Pathway.Models;
using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class CourseSectionService : ICourseSectionService
    {
        private readonly ICourseSectionRepository _sectionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;

        public CourseSectionService(
            ICourseSectionRepository sectionRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository)
        {
            _sectionRepository = sectionRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
        }

        public async Task<CourseSectionsPageViewModel?> GetSectionsForCourseAsync(int courseId, int requestingUserId, bool isAdmin)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return null;

            if (!isAdmin && course.InstructorId != requestingUserId)
                return null;

            var sections = await _sectionRepository.GetByCourseIdAsync(courseId);

            return new CourseSectionsPageViewModel
            {
                CourseId = courseId,
                CourseTitle = course.Title,
                Sections = sections.Select(s => new CourseSectionViewModel
                {
                    SectionId = s.SectionId,
                    Title = s.Title,
                    CourseId = s.CourseId,
                    Order = s.Order,
                    LessonCount = s.Lessons?.Count ?? 0
                }).ToList()
            };
        }

        public async Task<CourseSectionFormViewModel?> GetForEditAsync(int sectionId, int requestingUserId, bool isAdmin)
        {
            var section = await _sectionRepository.GetByIdWithCourseAsync(sectionId);
            if (section == null)
                return null;

            if (!isAdmin && section.Course.InstructorId != requestingUserId)
                return null;

            return new CourseSectionFormViewModel
            {
                SectionId = section.SectionId,
                CourseId = section.CourseId,
                Title = section.Title,
                Order = section.Order
            };
        }

        public async Task<SectionResult> CreateAsync(int courseId, int requestingUserId, bool isAdmin, CourseSectionFormViewModel model)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return SectionResult.Fail("Course not found.");

            if (!isAdmin && course.InstructorId != requestingUserId)
                return SectionResult.Fail("You are not allowed to add sections to this course.");

            var section = new CourseSection
            {
                CourseId = courseId,
                Title = model.Title,
                Order = model.Order
            };

            await _sectionRepository.AddAsync(section);
            await _sectionRepository.SaveChangesAsync();

            return SectionResult.Ok("Section created successfully.", section.SectionId, courseId);
        }

        public async Task<SectionResult> UpdateAsync(int sectionId, int requestingUserId, bool isAdmin, CourseSectionFormViewModel model)
        {
            var section = await _sectionRepository.GetByIdWithCourseAsync(sectionId);
            if (section == null)
                return SectionResult.Fail("Section not found.");

            if (!isAdmin && section.Course.InstructorId != requestingUserId)
                return SectionResult.Fail("You are not allowed to edit this section.");

            section.Title = model.Title;
            section.Order = model.Order;

            _sectionRepository.Update(section);
            await _sectionRepository.SaveChangesAsync();

            return SectionResult.Ok("Section updated successfully.", section.SectionId, section.CourseId);
        }

        public async Task<SectionResult> DeleteAsync(int sectionId, int requestingUserId, bool isAdmin)
        {
            var section = await _sectionRepository.GetByIdWithCourseAsync(sectionId);
            if (section == null)
                return SectionResult.Fail("Section not found.");

            if (!isAdmin && section.Course.InstructorId != requestingUserId)
                return SectionResult.Fail("You are not allowed to delete this section.");

            var courseId = section.CourseId;

            // Lessons cascade-delete automatically (see AppDbContext).
            _sectionRepository.Delete(section);
            await _sectionRepository.SaveChangesAsync();

            return SectionResult.Ok("Section deleted successfully.", null, courseId);
        }

        public async Task<CourseContentBuilderViewModel?> GetContentBuilderAsync(int courseId, int requestingUserId, bool isAdmin)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return null;

            if (!isAdmin && course.InstructorId != requestingUserId)
                return null;

            return new CourseContentBuilderViewModel
            {
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                Sections = new List<ContentBuilderSectionViewModel>
                {
                    new ContentBuilderSectionViewModel
                    {
                        Lessons = new List<ContentBuilderLessonViewModel> { new ContentBuilderLessonViewModel() }
                    }
                }
            };
        }

        public async Task<SectionResult> SaveContentAsync(int courseId, int requestingUserId, bool isAdmin, CourseContentBuilderViewModel model)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return SectionResult.Fail("Course not found.");

            if (!isAdmin && course.InstructorId != requestingUserId)
                return SectionResult.Fail("You are not allowed to add content to this course.");

            var existingSections = await _sectionRepository.GetByCourseIdAsync(courseId);
            var nextSectionOrder = existingSections.Any() ? existingSections.Max(s => s.Order) + 1 : 1;

            var sectionsCreated = 0;
            var lessonsCreated = 0;

            foreach (var sectionInput in model.Sections)
            {
                if (string.IsNullOrWhiteSpace(sectionInput.Title))
                    continue;

                var section = new CourseSection
                {
                    CourseId = courseId,
                    Title = sectionInput.Title.Trim(),
                    Order = nextSectionOrder++
                };

                await _sectionRepository.AddAsync(section);
                await _sectionRepository.SaveChangesAsync();
                sectionsCreated++;

                var lessonOrder = 1;
                foreach (var lessonInput in sectionInput.Lessons)
                {
                    if (string.IsNullOrWhiteSpace(lessonInput.Title))
                        continue;

                    var lesson = new Lesson
                    {
                        SectionId = section.SectionId,
                        Title = lessonInput.Title.Trim(),
                        VideoUrl = string.IsNullOrWhiteSpace(lessonInput.VideoUrl) ? null : lessonInput.VideoUrl.Trim(),
                        Order = lessonOrder++
                    };

                    await _lessonRepository.AddAsync(lesson);
                    lessonsCreated++;
                }
            }

            if (lessonsCreated > 0)
                await _lessonRepository.SaveChangesAsync();

            if (sectionsCreated == 0)
                return SectionResult.Fail("Please add at least one section with a title.");

            return SectionResult.Ok($"Added {sectionsCreated} section(s) and {lessonsCreated} lesson(s).", null, courseId);
        }
    }
}
