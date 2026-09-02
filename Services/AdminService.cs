using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        private static readonly string[] ValidRoles = { "Admin", "Instructor", "Student" };

        public AdminService(
            IUserRepository userRepository,
            ICourseRepository courseRepository,
            ICategoryRepository categoryRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<AdminDashboardViewModel> GetDashboardAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var courses = await _courseRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllWithCourseCountAsync();
            var enrollments = await _enrollmentRepository.GetAllAsync();

            return new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                TotalStudents = users.Count(u => u.Role == "Student"),
                TotalInstructors = users.Count(u => u.Role == "Instructor"),
                TotalCourses = courses.Count,
                TotalCategories = categories.Count,
                TotalEnrollments = enrollments.Count
            };
        }

        public async Task<List<AdminUserListItemViewModel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new AdminUserListItemViewModel
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<AdminUserDetailsViewModel?> GetUserDetailsAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithDetailsAsync(userId);
            if (user == null)
                return null;

            var model = new AdminUserDetailsViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            if (user.Role == "Instructor" && user.Courses != null)
            {
                model.Courses = user.Courses.Select(c => new AdminUserCourseItem
                {
                    CourseId = c.CourseId,
                    Title = c.Title
                }).ToList();
            }
            else if (user.Role == "Student" && user.Enrollments != null)
            {
                model.Enrollments = user.Enrollments.Select(e => new AdminUserEnrollmentItem
                {
                    CourseId = e.CourseId,
                    CourseTitle = e.Course?.Title ?? string.Empty,
                    Progress = e.Progress
                }).ToList();
            }

            return model;
        }

        public async Task<AdminUserEditViewModel?> GetUserForEditAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            return new AdminUserEditViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AdminResult> UpdateUserAsync(int userId, AdminUserEditViewModel model)
        {
            if (!ValidRoles.Contains(model.Role))
                return AdminResult.Fail("Invalid role selected.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return AdminResult.Fail("User not found.");

            var emailTaken = await _userRepository.EmailExistsAsync(model.Email, userId);
            if (emailTaken)
                return AdminResult.Fail("This email is already used by another account.");

            // Changing an Instructor's role away from Instructor while they still own
            // courses would leave orphaned data references, so block it.
            if (user.Role == "Instructor" && model.Role != "Instructor")
            {
                var hasCourses = await _userRepository.HasCoursesAsync(userId);
                if (hasCourses)
                    return AdminResult.Fail("Cannot change role: this instructor still has courses assigned.");
            }

            if (user.Role == "Student" && model.Role != "Student")
            {
                var hasEnrollments = await _userRepository.HasEnrollmentsAsync(userId);
                if (hasEnrollments)
                    return AdminResult.Fail("Cannot change role: this student still has active enrollments.");
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.Role = model.Role;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return AdminResult.Ok("User updated successfully.");
        }

        public async Task<AdminResult> DeleteUserAsync(int userId, int requestingAdminId)
        {
            if (userId == requestingAdminId)
                return AdminResult.Fail("You cannot delete your own account while logged in as it.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return AdminResult.Fail("User not found.");

            if (user.Role == "Instructor")
            {
                var hasCourses = await _userRepository.HasCoursesAsync(userId);
                if (hasCourses)
                    return AdminResult.Fail("Cannot delete this instructor: they still have courses. Reassign or delete those courses first.");
            }

            if (user.Role == "Student")
            {
                var hasEnrollments = await _userRepository.HasEnrollmentsAsync(userId);
                if (hasEnrollments)
                    return AdminResult.Fail("Cannot delete this student: they still have active enrollments.");
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return AdminResult.Ok("User deleted successfully.");
        }

        public async Task<AdminStatisticsViewModel> GetStatisticsAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var courses = await _courseRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllWithCourseCountAsync();
            var enrollments = await _enrollmentRepository.GetAllAsync();

            return new AdminStatisticsViewModel
            {
                TotalUsers = users.Count,
                TotalStudents = users.Count(u => u.Role == "Student"),
                TotalInstructors = users.Count(u => u.Role == "Instructor"),
                TotalAdmins = users.Count(u => u.Role == "Admin"),
                TotalCourses = courses.Count,
                TotalCategories = categories.Count,
                TotalEnrollments = enrollments.Count,
                AverageCoursePrice = courses.Any() ? courses.Average(c => c.Price) : 0,
                AverageProgress = enrollments.Any() ? enrollments.Average(e => e.Progress) : 0,
                CoursesByCategory = categories
                    .Select(c => new CategoryStat { CategoryName = c.Name, CourseCount = c.Courses?.Count ?? 0 })
                    .OrderByDescending(c => c.CourseCount)
                    .ToList(),
                TopInstructors = courses
                    .GroupBy(c => c.Instructor?.Name ?? "Unknown")
                    .Select(g => new InstructorStat { InstructorName = g.Key, CourseCount = g.Count() })
                    .OrderByDescending(i => i.CourseCount)
                    .Take(5)
                    .ToList()
            };
        }
    }
}
