using Pathway.Models;
using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<StudentDashboardViewModel> GetStudentDashboardAsync(int studentId)
        {
            var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
            var enrollmentViewModels = enrollments.Select(MapToViewModel).ToList();

            var completedCount = enrollments.Count(e => e.Progress >= 100);
            var totalCount = enrollments.Count;

            return new StudentDashboardViewModel
            {
                TotalCourses = totalCount,
                CompletedCourses = completedCount,
                InProgressCourses = totalCount - completedCount,
                AverageProgress = totalCount == 0 ? 0 : enrollments.Average(e => e.Progress),
                Enrollments = enrollmentViewModels
            };
        }

        public async Task<EnrollmentViewModel?> GetDetailsAsync(int enrollmentId, int studentId)
        {
            var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId);

            // IDOR protection: a student can only view their own enrollment
            if (enrollment == null || enrollment.StudentId != studentId)
                return null;

            return MapToViewModel(enrollment);
        }

        public async Task<EnrollmentResult> EnrollAsync(int studentId, int courseId)
        {
            var alreadyEnrolled = await _enrollmentRepository.ExistsAsync(studentId, courseId);
            if (alreadyEnrolled)
                return EnrollmentResult.Fail("You are already enrolled in this course.");

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrolledAt = DateTime.UtcNow,
                Progress = 0
            };

            await _enrollmentRepository.AddAsync(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return EnrollmentResult.Ok("Enrolled successfully.", enrollment.EnrollmentId);
        }

        public async Task<EnrollmentResult> UpdateProgressAsync(int enrollmentId, int studentId, int progress)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);

            if (enrollment == null || enrollment.StudentId != studentId)
                return EnrollmentResult.Fail("Enrollment not found.");

            if (progress < 0 || progress > 100)
                return EnrollmentResult.Fail("Progress must be between 0 and 100.");

            enrollment.Progress = progress;
            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return EnrollmentResult.Ok("Progress updated.");
        }

        public async Task<EnrollmentResult> CancelAsync(int enrollmentId, int studentId)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);

            if (enrollment == null || enrollment.StudentId != studentId)
                return EnrollmentResult.Fail("Enrollment not found.");

            _enrollmentRepository.Delete(enrollment);
            await _enrollmentRepository.SaveChangesAsync();

            return EnrollmentResult.Ok("Enrollment cancelled.");
        }

        private static EnrollmentViewModel MapToViewModel(Enrollment enrollment)
        {
            return new EnrollmentViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course?.Title ?? string.Empty,
                InstructorName = enrollment.Course?.Instructor?.Name ?? string.Empty,
                CategoryName = enrollment.Course?.Category?.Name ?? string.Empty,
                Price = enrollment.Course?.Price ?? 0,
                EnrolledAt = enrollment.EnrolledAt,
                Progress = enrollment.Progress
            };
        }
    }
}