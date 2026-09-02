using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Repositories.Interfaces;

namespace Pathway.Repositories
{
    public class CourseSectionRepository : ICourseSectionRepository
    {
        private readonly AppDbContext _context;

        public CourseSectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseSection>> GetByCourseIdAsync(int courseId)
        {
            return await _context.CourseSections
                .Where(s => s.CourseId == courseId)
                .Include(s => s.Lessons)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<CourseSection?> GetByIdAsync(int sectionId)
        {
            return await _context.CourseSections.FindAsync(sectionId);
        }

        public async Task<CourseSection?> GetByIdWithCourseAsync(int sectionId)
        {
            return await _context.CourseSections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.SectionId == sectionId);
        }

        public async Task AddAsync(CourseSection section)
        {
            await _context.CourseSections.AddAsync(section);
        }

        public void Update(CourseSection section)
        {
            _context.CourseSections.Update(section);
        }

        public void Delete(CourseSection section)
        {
            _context.CourseSections.Remove(section);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
