using Pathway.ViewModels;

namespace Pathway.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterViewModel model);
        Task<AuthResult> ValidateLoginAsync(LoginViewModel model);
        Task<ProfileViewModel?> GetProfileAsync(int userId);
    }
}
