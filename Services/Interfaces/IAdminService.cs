using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardAsync();
        Task<List<AdminUserListItemViewModel>> GetAllUsersAsync();
        Task<AdminUserDetailsViewModel?> GetUserDetailsAsync(int userId);
        Task<AdminUserEditViewModel?> GetUserForEditAsync(int userId);
        Task<AdminResult> UpdateUserAsync(int userId, AdminUserEditViewModel model);
        Task<AdminResult> DeleteUserAsync(int userId, int requestingAdminId);
        Task<AdminStatisticsViewModel> GetStatisticsAsync();
    }
}
