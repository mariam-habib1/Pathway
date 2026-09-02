using Pathway.Models;
using Pathway.Repositories.Interfaces;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryViewModel>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllWithCourseCountAsync();

            return categories.Select(c => new CategoryViewModel
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CourseCount = c.Courses?.Count ?? 0
            }).ToList();
        }

        public async Task<CategoryDetailsViewModel?> GetDetailsAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdWithCoursesAsync(categoryId);
            if (category == null)
                return null;

            return new CategoryDetailsViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                Courses = category.Courses.Select(course => new CategoryCourseViewModel
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    ThumbnailUrl = course.ThumbnailUrl,
                    InstructorName = course.Instructor?.Name ?? string.Empty,
                    Price = course.Price
                }).ToList()
            };
        }

        public async Task<CategoryFormViewModel?> GetForEditAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return null;

            return new CategoryFormViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryResult> CreateAsync(CategoryFormViewModel model)
        {
            var nameTaken = await _categoryRepository.NameExistsAsync(model.Name);
            if (nameTaken)
                return CategoryResult.Fail("A category with this name already exists.");

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return CategoryResult.Ok("Category created successfully.", category.CategoryId);
        }

        public async Task<CategoryResult> UpdateAsync(int categoryId, CategoryFormViewModel model)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return CategoryResult.Fail("Category not found.");

            var nameTaken = await _categoryRepository.NameExistsAsync(model.Name, categoryId);
            if (nameTaken)
                return CategoryResult.Fail("A category with this name already exists.");

            category.Name = model.Name;
            category.Description = model.Description;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return CategoryResult.Ok("Category updated successfully.");
        }

        public async Task<CategoryResult> DeleteAsync(int categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return CategoryResult.Fail("Category not found.");

            var hasCourses = await _categoryRepository.HasCoursesAsync(categoryId);
            if (hasCourses)
                return CategoryResult.Fail("Cannot delete a category that still has courses assigned to it.");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return CategoryResult.Ok("Category deleted successfully.");
        }
    }
}
