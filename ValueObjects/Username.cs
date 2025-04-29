using ValueObjects.Base;
using ValueObjects.Validators;

namespace ValueObjects
{
    public class Username: ValueObject
    {
        public string Value { get; }

        public Username(string value)
        {
            // Сначала валидируем входную строку
            new UsernameValidator().Validate(value);

            // Затем присваиваем значение
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues() => new[] { Value };

        public static implicit operator string(Username username) => username?.Value;
        public static explicit operator Username(string value) => new(value);
    }
}
