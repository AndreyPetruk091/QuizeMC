using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public class AnswerIndex : ValueObject
    {
        public int Value { get; }

        // Конструктор для EF Core
        protected AnswerIndex() { }

        public AnswerIndex(int value)
        {
            if (value < 0)
                throw new AnswerIndexException("Индекс не может быть отрицательным");

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}