using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class CategoryDescription : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 500;

        // Конструктор для EF Core
        private CategoryDescription() { }

        public CategoryDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = string.Empty;
                return;
            }

            if (value.Length > MaxLength)
                throw new ValidationException($"Category description cannot exceed {MaxLength} characters");

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(CategoryDescription description) => description.Value;
    }
}