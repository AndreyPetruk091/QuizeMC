using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace QuizeMC.Domain.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;

        public PasswordHash HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AdminException("Password cannot be empty");

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256);

            var salt = algorithm.Salt;
            var key = algorithm.GetBytes(KeySize);

            var hash = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hash, 0, SaltSize);
            Array.Copy(key, 0, hash, SaltSize, KeySize);

            var base64Hash = Convert.ToBase64String(hash);

            return new PasswordHash(base64Hash);
        }

        public bool VerifyPassword(string password, PasswordHash passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            var hashBytes = Convert.FromBase64String(passwordHash.Value);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            var key = algorithm.GetBytes(KeySize);

            for (var i = 0; i < KeySize; i++)
            {
                if (hashBytes[i + SaltSize] != key[i])
                    return false;
            }

            return true;
        }

        public string GenerateStrongPassword()
        {
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()_-+=<>?";

            var allChars = uppercase + lowercase + numbers + symbols;
            var random = new Random();

            var password = new char[12];

            // Гарантируем наличие хотя бы одного символа каждого типа
            password[0] = uppercase[random.Next(uppercase.Length)];
            password[1] = lowercase[random.Next(lowercase.Length)];
            password[2] = numbers[random.Next(numbers.Length)];
            password[3] = symbols[random.Next(symbols.Length)];

            // Заполняем остальные символы
            for (int i = 4; i < 12; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }

            // Перемешиваем символы
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        public bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            var hasUpper = password.Any(char.IsUpper);
            var hasLower = password.Any(char.IsLower);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}