using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get; }

        // Конструктор для EF Core
        private PasswordHash() { }

        public PasswordHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ValidationException("Password hash cannot be empty");

            if (hash.Length < 60) // Минимальная длина BCrypt хеша
                throw new ValidationException("Invalid password hash format");

            Value = hash;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => "[HIDDEN]";
    }
}