
namespace QuizeMC.Domain.ValueObjects.Base

{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public bool Equals(ValueObject other) => Equals((object)other);

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            return GetAtomicValues()
                .SequenceEqual(((ValueObject)obj).GetAtomicValues());
        }

        public override int GetHashCode() =>
            GetAtomicValues()
                .Aggregate(17, (current, obj) =>
                    current * 23 + (obj?.GetHashCode() ?? 0));

        public static bool operator ==(ValueObject left, ValueObject right) => Equals(left, right);
        public static bool operator !=(ValueObject left, ValueObject right) => !Equals(left, right);
    }
}