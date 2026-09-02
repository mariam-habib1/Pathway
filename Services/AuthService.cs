using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        private static readonly string[] AllowedSelfRegisterRoles = { "Student", "Instructor" };

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
        {
            if (!AllowedSelfRegisterRoles.Contains(model.Role))
                return AuthResult.Fail("Invalid role selected.");

            var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
                return AuthResult.Fail("An account with this email already exists.");

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = string.Empty
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return AuthResult.Ok(user.UserId, user.Name, user.Role, "Account created successfully.");
        }

        public async Task<AuthResult> ValidateLoginAsync(LoginViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return AuthResult.Fail("Invalid email or password.");

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verification == PasswordVerificationResult.Failed)
                return AuthResult.Fail("Invalid email or password.");

            return AuthResult.Ok(user.UserId, user.Name, user.Role);
        }

        public async Task<ProfileViewModel?> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            return new ProfileViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
