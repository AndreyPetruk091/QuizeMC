using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;

namespace QuizeMC.Domain.Entities
{
    public class Category : EntityBase
    {
        public CategoryName Name { get; private set; }
        public CategoryDescription Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedByAdminId { get; private set; }

        // Навигационные свойства
        public virtual Admin CreatedByAdmin { get; private set; }
        private readonly List<Quiz> _quizzes = new();
        public virtual IReadOnlyCollection<Quiz> Quizzes => _quizzes.AsReadOnly();

        // Конструктор для EF Core
        protected Category() { }

        public Category(CategoryName name, CategoryDescription description, Admin createdByAdmin)
        {
            Name = name ?? throw new DomainException("Category name cannot be null.");
            Description = description ?? throw new DomainException("Category description cannot be null.");
            CreatedByAdmin = createdByAdmin ?? throw new DomainException("Admin cannot be null.");
            CreatedByAdminId = createdByAdmin.Id;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateName(CategoryName newName)
        {
            Name = newName ?? throw new DomainException("Category name cannot be null.");
        }

        public void UpdateDescription(CategoryDescription newDescription)
        {
            Description = newDescription ?? throw new DomainException("Category description cannot be null.");
        }

        public void AddQuiz(Quiz quiz)
        {
            _quizzes.Add(quiz ?? throw new DomainException("Quiz cannot be null."));
        }

        public void RemoveQuiz(Quiz quiz)
        {
            _quizzes.Remove(quiz);
        }

        public int GetQuizzesCount() => _quizzes.Count;
    }
}