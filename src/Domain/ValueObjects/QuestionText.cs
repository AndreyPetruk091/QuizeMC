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
                throw new QuestionTextException(ExceptionMessages.NullValue);

            if (value.Length > MaxLength)
                throw new QuestionTextException(
                    ExceptionMessages.Format(ExceptionMessages.NullValue, MaxLength));

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(QuestionText text) => text.Value;
    }
}