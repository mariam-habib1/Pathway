using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Repositories.Interfaces;

namespace Pathway.Repositories
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
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Include(c => c.Sections)
                    .ThenInclude(s => s.Lessons)
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<List<Course>> GetByInstructorIdAsync(int instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Course>> SearchAsync(string keyword)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Where(c => c.Title.Contains(keyword) ||
                            (c.Description != null && c.Description.Contains(keyword)))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Course>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Courses
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .OrderByDescending(c => c.CreatedAt)
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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
