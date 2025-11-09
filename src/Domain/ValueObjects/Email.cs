using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 255;

        // Конструктор для EF Core
        private Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Email cannot be empty");

            if (value.Length > MaxLength)
                throw new ValidationException($"Email cannot exceed {MaxLength} characters");

            if (!IsValidEmail(value))
                throw new ValidationException("Invalid email format");

            Value = value.Trim().ToLower();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(Email email) => email.Value;
    }
}