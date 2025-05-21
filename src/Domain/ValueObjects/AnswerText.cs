using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class AnswerText : ValueObject
    {
        public string Value { get; }

        // Конструктор для EF Core
        private AnswerText() { }

        public AnswerText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Answer text cannot be empty");

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}