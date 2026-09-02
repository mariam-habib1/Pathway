using Microsoft.AspNetCore.WebUtilities;
using Pathway.Models;
using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseSectionRepository _sectionRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public LessonService(
            ILessonRepository lessonRepository,
            ICourseSectionRepository sectionRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _lessonRepository = lessonRepository;
            _sectionRepository = sectionRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        // Owner instructor, Admin, or a student enrolled in the parent course
        private async Task<bool> HasReadAccessAsync(Course course, int userId, bool isAdmin)
        {
            if (isAdmin || course.InstructorId == userId)
                return true;

            return await _enrollmentRepository.ExistsAsync(userId, course.CourseId);
        }

        private static bool HasWriteAccess(Course course, int userId, bool isAdmin)
        {
            return isAdmin || course.InstructorId == userId;
        }

        public async Task<LessonsPageViewModel?> GetLessonsForSectionAsync(int sectionId, int requestingUserId, bool isAdmin)
        {
            var section = await _sectionRepository.GetByIdWithCourseAsync(sectionId);
            if (section == null)
                return null;

            if (!await HasReadAccessAsync(section.Course, requestingUserId, isAdmin))
                return null;

            var lessons = await _lessonRepository.GetBySectionIdAsync(sectionId);

            return new LessonsPageViewModel
            {
                SectionId = section.SectionId,
                SectionTitle = section.Title,
                CourseId = section.CourseId,
                CanManage = HasWriteAccess(section.Course, requestingUserId, isAdmin),
                Lessons = lessons.Select(l => new LessonViewModel
                {
                    LessonId = l.LessonId,
                    Title = l.Title,
                    SectionId = l.SectionId,
                    Order = l.Order
                }).ToList()
            };
        }

        // A YouTube "watch" or "youtu.be" link can't be loaded in an <iframe> — YouTube blocks
        // framing for those pages. Only the /embed/{id} form is embeddable, so convert on display.
        private static string? ToEmbeddableVideoUrl(string? videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
                return videoUrl;

            if (!Uri.TryCreate(videoUrl, UriKind.Absolute, out var uri))
                return videoUrl;

            var host = uri.Host.ToLowerInvariant();
            if (!host.Contains("youtube.com") && !host.Contains("youtu.be"))
                return videoUrl;

            if (uri.AbsolutePath.StartsWith("/embed/", StringComparison.OrdinalIgnoreCase))
                return videoUrl;

            string? videoId = null;

            if (host.Contains("youtu.be"))
            {
                videoId = uri.AbsolutePath.Trim('/');
            }
            else if (uri.AbsolutePath.StartsWith("/shorts/", StringComparison.OrdinalIgnoreCase))
            {
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                videoId = segments.Length > 1 ? segments[1] : null;
            }
            else
            {
                var query = QueryHelpers.ParseQuery(uri.Query);
                videoId = query.TryGetValue("v", out var v) ? v.ToString() : null;
            }

            return string.IsNullOrWhiteSpace(videoId)
                ? videoUrl
                : $"https://www.youtube.com/embed/{videoId}";
        }

        public async Task<LessonDetailsViewModel?> GetLessonDetailsAsync(int lessonId, int requestingUserId, bool isAdmin)
        {
            var lesson = await _lessonRepository.GetByIdWithSectionAsync(lessonId);
            if (lesson == null)
                return null;

            if (!await HasReadAccessAsync(lesson.Section.Course, requestingUserId, isAdmin))
                return null;

            return new LessonDetailsViewModel
            {
                LessonId = lesson.LessonId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = ToEmbeddableVideoUrl(lesson.VideoUrl),
                Order = lesson.Order,
                SectionId = lesson.SectionId,
                SectionTitle = lesson.Section.Title,
                CourseId = lesson.Section.CourseId,
                CourseTitle = lesson.Section.Course.Title
            };
        }

        public async Task<LessonFormViewModel?> GetForEditAsync(int lessonId, int requestingUserId, bool isAdmin)
        {
            var lesson = await _lessonRepository.GetByIdWithSectionAsync(lessonId);
            if (lesson == null)
                return null;

            if (!HasWriteAccess(lesson.Section.Course, requestingUserId, isAdmin))
                return null;

            return new LessonFormViewModel
            {
                LessonId = lesson.LessonId,
                SectionId = lesson.SectionId,
                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                Order = lesson.Order
            };
        }

        public async Task<LessonResult> CreateAsync(int sectionId, int requestingUserId, bool isAdmin, LessonFormViewModel model)
        {
            var section = await _sectionRepository.GetByIdWithCourseAsync(sectionId);
            if (section == null)
                return LessonResult.Fail("Section not found.");

            if (!HasWriteAccess(section.Course, requestingUserId, isAdmin))
                return LessonResult.Fail("You are not allowed to add lessons to this section.");

            var lesson = new Lesson
            {
                SectionId = sectionId,
                Title = model.Title,
                Content = model.Content,
                VideoUrl = model.VideoUrl,
                Order = model.Order
            };

            await _lessonRepository.AddAsync(lesson);
            await _lessonRepository.SaveChangesAsync();

            return LessonResult.Ok("Lesson created successfully.", lesson.LessonId, sectionId);
        }

        public async Task<LessonResult> UpdateAsync(int lessonId, int requestingUserId, bool isAdmin, LessonFormViewModel model)
        {
            var lesson = await _lessonRepository.GetByIdWithSectionAsync(lessonId);
            if (lesson == null)
                return LessonResult.Fail("Lesson not found.");

            if (!HasWriteAccess(lesson.Section.Course, requestingUserId, isAdmin))
                return LessonResult.Fail("You are not allowed to edit this lesson.");

            lesson.Title = model.Title;
            lesson.Content = model.Content;
            lesson.VideoUrl = model.VideoUrl;
            lesson.Order = model.Order;

            _lessonRepository.Update(lesson);
            await _lessonRepository.SaveChangesAsync();

            return LessonResult.Ok("Lesson updated successfully.", lesson.LessonId, lesson.SectionId);
        }

        public async Task<LessonResult> DeleteAsync(int lessonId, int requestingUserId, bool isAdmin)
        {
            var lesson = await _lessonRepository.GetByIdWithSectionAsync(lessonId);
            if (lesson == null)
                return LessonResult.Fail("Lesson not found.");

            if (!HasWriteAccess(lesson.Section.Course, requestingUserId, isAdmin))
                return LessonResult.Fail("You are not allowed to delete this lesson.");

            var sectionId = lesson.SectionId;

            _lessonRepository.Delete(lesson);
            await _lessonRepository.SaveChangesAsync();

            return LessonResult.Ok("Lesson deleted successfully.", null, sectionId);
        }
    }
}
