using QuizeMC.Domain.ValueObjects.Base;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class Answer : ValueObject
    {
        public AnswerText Text { get; }

        // Конструктор для EF Core
        private Answer() { }

        public Answer(AnswerText text)
        {
            Text = text;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
        }
    }
}