using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class QuizDescription : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 1000;

        // Конструктор для EF Core
        private QuizDescription() { }

        public QuizDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = string.Empty;
                return;
            }

            if (value.Length > MaxLength)
                throw new ValidationException($"Quiz description cannot exceed {MaxLength} characters");

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(QuizDescription description) => description.Value;
    }
}