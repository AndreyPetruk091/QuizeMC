using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects
{
    public sealed class CategoryName : ValueObject
    {
        public string Value { get; }
        private const int MaxLength = 100;

        // Конструктор для EF Core
        private CategoryName() { }

        public CategoryName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException("Category name cannot be empty");

            if (value.Length > MaxLength)
                throw new ValidationException($"Category name cannot exceed {MaxLength} characters");

            Value = value.Trim();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(CategoryName name) => name.Value;
    }
}