using ValueObjects.Base;


namespace ValueObjects
{
    public sealed class QuizTitle : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 100;

        public QuizTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MaxLength)
                throw new ArgumentException($"Title must be 1-{MaxLength} characters");

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(QuizTitle title) => title.Value;
    }
}