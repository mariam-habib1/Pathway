using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Repositories.Interfaces;

namespace Pathway.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetBySectionIdAsync(int sectionId)
        {
            return await _context.Lessons
                .Where(l => l.SectionId == sectionId)
                .OrderBy(l => l.Order)
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int lessonId)
        {
            return await _context.Lessons.FindAsync(lessonId);
        }

        public async Task<Lesson?> GetByIdWithSectionAsync(int lessonId)
        {
            return await _context.Lessons
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
        }

        public async Task AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
        }

        public void Update(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
        }

        public void Delete(Lesson lesson)
        {
            _context.Lessons.Remove(lesson);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
