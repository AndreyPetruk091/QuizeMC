using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Services.Abstractions
{
    public interface IAdminService
    {
        Task<Admin> CreateAdminAsync(Email email, string password);
        Task<bool> ValidateAdminCredentialsAsync(Email email, string password);
        Task ChangeAdminPasswordAsync(Admin admin, string currentPassword, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(Admin admin);
        Task<bool> ResetPasswordAsync(Admin admin, string token, string newPassword);
    }
}