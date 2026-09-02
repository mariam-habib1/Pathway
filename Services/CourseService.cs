using Microsoft.AspNetCore.Mvc.Rendering;
using Pathway.Models;
using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository, IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<List<CourseViewModel>> GetAllAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToViewModel).ToList();
        }

        public async Task<CourseDetailsViewModel?> GetDetailsAsync(int courseId, int? currentUserId = null)
        {
            var course = await _courseRepository.GetByIdWithDetailsAsync(courseId);
            if (course == null)
                return null;

            var isEnrolled = currentUserId.HasValue &&
                await _enrollmentRepository.ExistsAsync(currentUserId.Value, courseId);

            var isOwner = currentUserId.HasValue && currentUserId.Value == course.InstructorId;

            return new CourseDetailsViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                ThumbnailUrl = course.ThumbnailUrl,
                Price = course.Price,
                IsEnrolled = isEnrolled,
                CanViewContent = isEnrolled || isOwner,
                Sections = course.Sections?
                    .OrderBy(s => s.Order)
                    .Select(s => new CourseSectionSummaryViewModel
                    {
                        SectionId = s.SectionId,
                        Title = s.Title,
                        Order = s.Order,
                        LessonCount = s.Lessons?.Count ?? 0,
                        Lessons = s.Lessons?
                            .OrderBy(l => l.Order)
                            .Select(l => new CourseLessonSummaryViewModel
                            {
                                LessonId = l.LessonId,
                                Title = l.Title,
                                Order = l.Order
                            }).ToList() ?? new List<CourseLessonSummaryViewModel>()
                    }).ToList() ?? new List<CourseSectionSummaryViewModel>()
            };
        }

        public async Task<CourseFormViewModel> GetEmptyFormAsync()
        {
            return new CourseFormViewModel
            {
                CategoryOptions = await GetCategoryOptionsAsync()
            };
        }

        public async Task<CourseFormViewModel?> GetForEditAsync(int courseId, int requestingUserId, bool isAdmin)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return null;

            if (!isAdmin && course.InstructorId != requestingUserId)
                return null;

            return new CourseFormViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                ThumbnailUrl = course.ThumbnailUrl,
                Price = course.Price,
                CategoryId = course.CategoryId,
                CategoryOptions = await GetCategoryOptionsAsync()
            };
        }

        public async Task<CourseResult> CreateAsync(int instructorId, CourseFormViewModel model)
        {
            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                ThumbnailUrl = model.ThumbnailUrl,
                Price = model.Price,
                CategoryId = model.CategoryId,
                InstructorId = instructorId,
                CreatedAt = DateTime.UtcNow
            };

            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            return CourseResult.Ok("Course created successfully.", course.CourseId);
        }

        public async Task<CourseResult> UpdateAsync(int courseId, int requestingUserId, bool isAdmin, CourseFormViewModel model)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return CourseResult.Fail("Course not found.");

            if (!isAdmin && course.InstructorId != requestingUserId)
                return CourseResult.Fail("You are not allowed to edit this course.");

            course.Title = model.Title;
            course.Description = model.Description;
            course.ThumbnailUrl = model.ThumbnailUrl;
            course.Price = model.Price;
            course.CategoryId = model.CategoryId;

            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();

            return CourseResult.Ok("Course updated successfully.");
        }

        public async Task<CourseResult> DeleteAsync(int courseId, int requestingUserId, bool isAdmin)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return CourseResult.Fail("Course not found.");

            if (!isAdmin && course.InstructorId != requestingUserId)
                return CourseResult.Fail("You are not allowed to delete this course.");

            // Sections, Lessons and Enrollments cascade-delete automatically (see AppDbContext).
            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync();

            return CourseResult.Ok("Course deleted successfully.");
        }

        public async Task<List<CourseViewModel>> GetByInstructorAsync(int instructorId)
        {
            var courses = await _courseRepository.GetByInstructorIdAsync(instructorId);
            return courses.Select(MapToViewModel).ToList();
        }

        public async Task<List<CourseViewModel>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllAsync();

            var courses = await _courseRepository.SearchAsync(keyword);
            return courses.Select(MapToViewModel).ToList();
        }

        public async Task<List<CourseViewModel>> GetByCategoryAsync(int categoryId)
        {
            var courses = await _courseRepository.GetByCategoryIdAsync(categoryId);
            return courses.Select(MapToViewModel).ToList();
        }

        private async Task<List<SelectListItem>> GetCategoryOptionsAsync()
        {
            var categories = await _categoryRepository.GetAllWithCourseCountAsync();
            return categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();
        }

        private static CourseViewModel MapToViewModel(Course course)
        {
            return new CourseViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                ThumbnailUrl = course.ThumbnailUrl,
                Price = course.Price,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor?.Name ?? string.Empty,
                CategoryName = course.Category?.Name ?? string.Empty,
                CreatedAt = course.CreatedAt
            };
        }
    }
}
