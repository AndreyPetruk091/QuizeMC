using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects
{
    public class AnswerIndex :ValueObject
    {
        public int Value { get; }

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


        public static implicit operator int(AnswerIndex index) => index.Value;
        public static explicit operator AnswerIndex(int value) => new(value);
        public override string ToString() => Value.ToString();
    }

}
