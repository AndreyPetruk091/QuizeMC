using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;

namespace QuizeMC.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(IAdminRepository adminRepository, IPasswordService passwordService)
        {
            _adminRepository = adminRepository;
            _passwordService = passwordService;
        }

        public async Task<Admin> CreateAdminAsync(Email email, string password)
        {
            // Проверяем, существует ли уже администратор с таким email
            var existingAdmin = await _adminRepository.GetByEmailAsync(email.Value);
            if (existingAdmin != null)
                throw new AdminException($"Admin with email {email.Value} already exists");

            // Проверяем сложность пароля
            if (!_passwordService.IsPasswordStrong(password))
                throw new AdminException("Password does not meet security requirements");

            // Хешируем пароль
            var passwordHash = _passwordService.HashPassword(password);

            // Создаем администратора
            var admin = new Admin(email, passwordHash);

            await _adminRepository.AddAsync(admin);
            return admin;
        }

        public async Task<bool> ValidateAdminCredentialsAsync(Email email, string password)
        {
            var admin = await _adminRepository.GetByEmailAsync(email.Value);
            if (admin == null || !admin.IsActive)
                return false;

            return _passwordService.VerifyPassword(password, admin.PasswordHash);
        }

        public async Task ChangeAdminPasswordAsync(Admin admin, string currentPassword, string newPassword)
        {
            // Проверяем текущий пароль
            if (!_passwordService.VerifyPassword(currentPassword, admin.PasswordHash))
                throw new AdminException("Current password is incorrect", admin.Id);

            // Проверяем сложность нового пароля
            if (!_passwordService.IsPasswordStrong(newPassword))
                throw new AdminException("New password does not meet security requirements");

            // Хешируем новый пароль
            var newPasswordHash = _passwordService.HashPassword(newPassword);

            admin.UpdatePassword(newPasswordHash);
            _adminRepository.Update(admin);
        }

        public Task<string> GeneratePasswordResetTokenAsync(Admin admin)
        {
            // Генерируем простой токен для сброса пароля
            var token = Guid.NewGuid().ToString("N") + DateTime.UtcNow.Ticks.ToString();
            return Task.FromResult(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token)));
        }

        public async Task<bool> ResetPasswordAsync(Admin admin, string token, string newPassword)
        {
            // В реальном приложении здесь была бы проверка токена
            // Для упрощения просто проверяем, что токен не пустой
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // Проверяем сложность нового пароля
            if (!_passwordService.IsPasswordStrong(newPassword))
                throw new AdminException("New password does not meet security requirements");

            // Хешируем новый пароль
            var newPasswordHash = _passwordService.HashPassword(newPassword);

            admin.UpdatePassword(newPasswordHash);
            _adminRepository.Update(admin);

            return true;
        }
    }
}