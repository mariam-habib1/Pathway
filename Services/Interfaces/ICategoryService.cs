using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllAsync();
        Task<CategoryDetailsViewModel?> GetDetailsAsync(int categoryId);
        Task<CategoryFormViewModel?> GetForEditAsync(int categoryId);
        Task<CategoryResult> CreateAsync(CategoryFormViewModel model);
        Task<CategoryResult> UpdateAsync(int categoryId, CategoryFormViewModel model);
        Task<CategoryResult> DeleteAsync(int categoryId);
    }
}
