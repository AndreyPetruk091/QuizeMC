using QuizeMC.Domain.Enums;
using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Entities
{
    public class Quiz : EntityBase
    {
        public QuizTitle Title { get; private set; }
        public QuizDescription Description { get; private set; }
        public QuizStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public DateTime? ArchivedAt { get; private set; }
        public Guid CreatedByAdminId { get; private set; }
        public Guid CategoryId { get; private set; }

        // Навигационные свойства
        public virtual Admin CreatedByAdmin { get; private set; }
        public virtual Category Category { get; private set; }

        private readonly List<Question> _questions = new();
        public virtual IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        private const int MaxQuestions = 100;

        // Конструктор для EF Core
        protected Quiz() { }

        public Quiz(QuizTitle title, QuizDescription description, Category category, Admin createdByAdmin)
        {
            Title = title ?? throw new QuizException("Quiz title cannot be null.");
            Description = description ?? throw new QuizException("Quiz description cannot be null.");
            Category = category ?? throw new QuizException("Category cannot be null.");
            CategoryId = category.Id;
            CreatedByAdmin = createdByAdmin ?? throw new QuizException("Admin cannot be null.");
            CreatedByAdminId = createdByAdmin.Id;
            Status = QuizStatus.Draft;
            CreatedAt = DateTime.UtcNow;
        }

        public void Publish()
        {
            if (Status == QuizStatus.Archived)
                throw new QuizException("Archived quiz cannot be published.");

            if (_questions.Count == 0)
                throw new QuizException("Cannot publish quiz without questions.");

            Status = QuizStatus.Active;
            PublishedAt = DateTime.UtcNow;
        }

        public void AddQuestion(Question question)
        {
            if (_questions.Count >= MaxQuestions)
                throw new QuizException($"Quiz cannot have more than {MaxQuestions} questions.");

            _questions.Add(question ?? throw new QuizException("Question cannot be null."));
        }

        public void RemoveQuestion(Question question)
        {
            _questions.Remove(question);
        }

        public void Archive()
        {
            Status = QuizStatus.Archived;
            ArchivedAt = DateTime.UtcNow;
        }

        public void UpdateTitle(QuizTitle newTitle)
        {
            Title = newTitle ?? throw new QuizException("Quiz title cannot be null.");
        }

        public void UpdateDescription(QuizDescription newDescription)
        {
            Description = newDescription ?? throw new QuizException("Quiz description cannot be null.");
        }

        public void UpdateCategory(Category newCategory)
        {
            Category = newCategory ?? throw new QuizException("Category cannot be null.");
            CategoryId = newCategory.Id;
        }

        public void MoveToDraft()
        {
            if (Status == QuizStatus.Archived)
                throw new QuizException("Archived quiz cannot be moved to draft.");

            Status = QuizStatus.Draft;
            PublishedAt = null;
        }

        public int GetQuestionsCount() => _questions.Count;
        public bool HasQuestions => _questions.Count > 0;
    }
}