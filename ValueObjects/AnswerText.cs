using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects
{
    public sealed class AnswerText : ValueObject
    {
        public string Value { get; }

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