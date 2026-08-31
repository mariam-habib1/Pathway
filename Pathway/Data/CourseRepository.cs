using Microsoft.EntityFrameworkCore;
using Pathway.Models;

namespace Pathway.Data
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<Course?> GetByIdWithSectionsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<List<Course>> GetByInstructorAsync(int instructorId)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .Where(c => c.InstructorId == instructorId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Course>> SearchAsync(string keyword)
        {
            keyword = keyword?.Trim().ToLower() ?? string.Empty;
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Where(c => c.Title.ToLower().Contains(keyword)
                         || (c.Description != null && c.Description.ToLower().Contains(keyword)))
                .ToListAsync();
        }

        public async Task<List<Course>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        public void Delete(Course course)
        {
            _context.Courses.Remove(course);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
