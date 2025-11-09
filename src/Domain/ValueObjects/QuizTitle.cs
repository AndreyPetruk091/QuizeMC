using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class QuizTitle : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 100;

        // Конструктор для EF Core
        private QuizTitle() { }

        public QuizTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Quiz title cannot be empty");

            if (value.Length > MaxLength)
                throw new ValidationException($"Quiz title cannot exceed {MaxLength} characters");

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(QuizTitle title) => title.Value;
    }
}