using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Repositories.Interfaces;

namespace Pathway.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllWithCourseCountAsync()
        {
            return await _context.Categories
                .Include(c => c.Courses)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<Category?> GetByIdWithCoursesAsync(int categoryId)
        {
            return await _context.Categories
                .Include(c => c.Courses)
                    .ThenInclude(course => course.Instructor)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeCategoryId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name &&
                               (excludeCategoryId == null || c.CategoryId != excludeCategoryId));
        }

        public async Task<bool> HasCoursesAsync(int categoryId)
        {
            return await _context.Courses.AnyAsync(course => course.CategoryId == categoryId);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
