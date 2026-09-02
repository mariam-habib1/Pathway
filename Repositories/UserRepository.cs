using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Repositories.Interfaces;

namespace Pathway.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetByIdWithDetailsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Courses)
                .Include(u => u.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && (excludeUserId == null || u.UserId != excludeUserId));
        }

        public async Task<bool> HasCoursesAsync(int instructorId)
        {
            return await _context.Courses.AnyAsync(c => c.InstructorId == instructorId);
        }

        public async Task<bool> HasEnrollmentsAsync(int studentId)
        {
            return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
