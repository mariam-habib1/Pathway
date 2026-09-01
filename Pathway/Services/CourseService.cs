using Pathway.Data;
using Pathway.Models;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;

        public CourseService(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<List<CourseListItemViewModel>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return courses.Select(MapToListItem).ToList();
        }

        public async Task<List<CourseListItemViewModel>> GetInstructorCoursesAsync(int instructorId)
        {
            var courses = await _courseRepo.GetByInstructorAsync(instructorId);
            return courses.Select(MapToListItem).ToList();
        }

        public async Task<CourseDetailsViewModel?> GetCourseDetailsAsync(int courseId)
        {
            var course = await _courseRepo.GetByIdWithSectionsAsync(courseId);
            if (course == null) return null;

            return new CourseDetailsViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                CategoryName = course.Category?.Name ?? "",
                InstructorName = course.Instructor?.Name ?? "",
                CreatedAt = course.CreatedAt,
                EnrollmentCount = course.Enrollments?.Count ?? 0,
                ImageUrl = course.ImageUrl,
                Sections = course.Sections?
                    .OrderBy(s => s.Order)
                    .Select(s => new SectionSummaryVM
                    {
                        SectionId = s.SectionId,
                        Title = s.Title,
                        Order = s.Order,
                        LessonCount = s.Lessons?.Count ?? 0
                    }).ToList() ?? new List<SectionSummaryVM>()
            };
        }

        public async Task<List<CourseListItemViewModel>> SearchCoursesAsync(string keyword)
        {
            var courses = await _courseRepo.SearchAsync(keyword);
            return courses.Select(MapToListItem).ToList();
        }

        public async Task<List<CourseListItemViewModel>> GetCoursesByCategoryAsync(int categoryId)
        {
            var courses = await _courseRepo.GetByCategoryAsync(categoryId);
            return courses.Select(MapToListItem).ToList();
        }

        public async Task<ServiceResult> CreateCourseAsync(CourseCreateViewModel model, int instructorId)
        {
            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                InstructorId = instructorId,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = model.ImageUrl
            };

            await _courseRepo.AddAsync(course);
            var saved = await _courseRepo.SaveChangesAsync();

            return saved ? ServiceResult.Ok() : ServiceResult.Fail("Could not create course.");
        }

        public async Task<Course?> GetCourseForEditAsync(int courseId, int currentUserId, bool isAdmin)
        {
            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course == null) return null;

            // Ownership check: only the owning instructor or an admin can edit
            if (!isAdmin && course.InstructorId != currentUserId)
                return null;

            return course;
        }

        public async Task<ServiceResult> UpdateCourseAsync(CourseEditViewModel model, int currentUserId, bool isAdmin)
        {
            var course = await _courseRepo.GetByIdAsync(model.CourseId);
            if (course == null)
                return ServiceResult.Fail("Course not found.");

            // Ownership check
            if (!isAdmin && course.InstructorId != currentUserId)
                return ServiceResult.Fail("You are not authorized to edit this course.");

            course.Title = model.Title;
            course.Description = model.Description;
            course.Price = model.Price;
            course.CategoryId = model.CategoryId;
            course.ImageUrl = model.ImageUrl;

            _courseRepo.Update(course);
            var saved = await _courseRepo.SaveChangesAsync();

            return saved ? ServiceResult.Ok() : ServiceResult.Fail("Could not update course.");
        }

        public async Task<ServiceResult> DeleteCourseAsync(int courseId, int currentUserId, bool isAdmin)
        {
            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course == null)
                return ServiceResult.Fail("Course not found.");

            if (!isAdmin && course.InstructorId != currentUserId)
                return ServiceResult.Fail("You are not authorized to delete this course.");

            _courseRepo.Delete(course);
            var saved = await _courseRepo.SaveChangesAsync();

            return saved ? ServiceResult.Ok() : ServiceResult.Fail("Could not delete course.");
        }

        private static CourseListItemViewModel MapToListItem(Course c) => new()
        {
            CourseId = c.CourseId,
            Title = c.Title,
            Description = c.Description,
            Price = c.Price,
            CategoryName = c.Category?.Name ?? "",
            InstructorName = c.Instructor?.Name ?? "",
            EnrollmentCount = c.Enrollments?.Count ?? 0,
            CreatedAt = c.CreatedAt,
            ImageUrl = c.ImageUrl
        };
    }
}
