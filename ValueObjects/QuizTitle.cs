using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects
{
    public class QuizTitle: ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 100;

        public QuizTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new QuizTitleException(ExeptionMasseges.EmptyQuizTitle);

            if (value.Length > MaxLength)
                throw new QuizTitleException(
                    ExeptionMasseges.Format(ExeptionMasseges.QuizTitleTooLong, MaxLength));

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues() => new[] { Value };
        public static implicit operator string(QuizTitle title) => title.Value;
    }
}
