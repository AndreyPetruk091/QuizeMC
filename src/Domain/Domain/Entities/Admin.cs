using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;

namespace QuizeMC.Domain.Entities
{
    public class Admin : EntityBase
    {
        public Email Email { get; private set; }
        public PasswordHash PasswordHash { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        // Навигационные свойства
        private readonly List<Quiz> _createdQuizzes = new();
        public virtual IReadOnlyCollection<Quiz> CreatedQuizzes => _createdQuizzes.AsReadOnly();

        private readonly List<Category> _createdCategories = new();
        public virtual IReadOnlyCollection<Category> CreatedCategories => _createdCategories.AsReadOnly();

        // Конструктор для EF Core
        protected Admin() { }

        public Admin(Email email, PasswordHash passwordHash)
        {
            Email = email ?? throw new DomainException("Email cannot be null.");
            PasswordHash = passwordHash ?? throw new DomainException("Password hash cannot be null.");
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateLoginTime()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void UpdateEmail(Email newEmail)
        {
            Email = newEmail ?? throw new DomainException("Email cannot be null.");
        }

        public void UpdatePassword(PasswordHash newPasswordHash)
        {
            PasswordHash = newPasswordHash ?? throw new DomainException("Password hash cannot be null.");
        }

        // Методы для управления сущностями
        public void AddCreatedQuiz(Quiz quiz)
        {
            _createdQuizzes.Add(quiz ?? throw new DomainException("Quiz cannot be null."));
        }

        public void AddCreatedCategory(Category category)
        {
            _createdCategories.Add(category ?? throw new DomainException("Category cannot be null."));
        }
    }
}