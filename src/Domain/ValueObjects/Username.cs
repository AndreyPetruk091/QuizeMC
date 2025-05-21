using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;
using QuizeMC.Domain.ValueObjects.Validators;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class Username : ValueObject
    {
        public string Value { get; }
        private static readonly UsernameValidator Validator = new();

        // Конструктор для EF Core
        private Username() { }

        public Username(string value)
        {
            Validator.Validate(value);
            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}