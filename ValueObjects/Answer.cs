using ValueObjects.Base;

namespace ValueObjects
{
    public sealed class Answer : ValueObject
    {
        private AnswerText answerText;

        public string Value { get; }

        public Answer(string value)
        {
            // Дефолтное значение вместо исключения
            Value = string.IsNullOrWhiteSpace(value) ? "Пустой ответ" : value;
        }

        public Answer(AnswerText answerText)
        {
            this.answerText = answerText;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(Answer answer) => answer.Value;
        public static explicit operator Answer(string value) => new(value);
    }
}