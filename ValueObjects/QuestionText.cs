using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects
{
    public class QuestionText: ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 500;

        public QuestionText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new  QuestionTextException(ExeptionMasseges.EmptyQuestionText);

            if (value.Length > MaxLength)
                throw new QuestionTextException(
                    ExeptionMasseges.Format(ExeptionMasseges.QuestionTextTooLong, MaxLength));

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues() => new[] { Value };
        public static implicit operator string(QuestionText text) => text.Value;
    }
}
