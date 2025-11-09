using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Services.Abstractions
{
    public interface IPasswordService
    {
        PasswordHash HashPassword(string password);
        bool VerifyPassword(string password, PasswordHash passwordHash);
        string GenerateStrongPassword();
        bool IsPasswordStrong(string password);
    }
}