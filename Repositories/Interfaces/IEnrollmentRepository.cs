using Pathway.Models;

namespace Pathway.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByIdAsync(int enrollmentId);
        Task<Enrollment?> GetByIdWithDetailsAsync(int enrollmentId);
        Task<List<Enrollment>> GetByStudentIdAsync(int studentId);
        Task<List<Enrollment>> GetAllAsync();
        Task<bool> ExistsAsync(int studentId, int courseId);
        Task AddAsync(Enrollment enrollment);
        void Update(Enrollment enrollment);
        void Delete(Enrollment enrollment);
        Task<int> SaveChangesAsync();
    }
}
