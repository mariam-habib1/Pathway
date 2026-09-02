using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<StudentDashboardViewModel> GetStudentDashboardAsync(int studentId);
        Task<EnrollmentViewModel?> GetDetailsAsync(int enrollmentId, int studentId);
        Task<EnrollmentResult> EnrollAsync(int studentId, int courseId);
        Task<EnrollmentResult> CancelAsync(int enrollmentId, int studentId);
    }
}
