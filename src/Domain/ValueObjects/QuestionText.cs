using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class QuestionText : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 500;

        // Конструктор для EF Core
        private QuestionText() { }

        public QuestionText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Question text cannot be empty");

            if (value.Length > MaxLength)
                throw new ValidationException($"Question text cannot exceed {MaxLength} characters");

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(QuestionText text) => text.Value;
    }
}