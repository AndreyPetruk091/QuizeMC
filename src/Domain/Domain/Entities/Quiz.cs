using QuizeMC.Domain.Enums; // Добавлена директива для QuizStatus
using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Entities
{
    public class Quiz : EntityBase
    {
        public QuizTitle Title { get; }
        public QuizStatus Status { get; private set; }
        private readonly List<Question> _questions = new();
        public virtual IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        private const int MaxQuestions = 100;

        // Конструктор для EF Core
        protected Quiz() { }

        public Quiz(QuizTitle title)
        {
            Title = title ?? throw new QuizException("Quiz title cannot be null.");
            Status = QuizStatus.Draft;
        }

        public void Publish()
        {
            if (Status == QuizStatus.Archived)
                throw new QuizException("Archived quiz cannot be published.");

            if (_questions.Count == 0)
                throw new QuizException("Cannot publish quiz without questions.");

            Status = QuizStatus.Active;
        }

        public void AddQuestion(Question question)
        {
            if (_questions.Count >= MaxQuestions)
                throw new QuizException($"Quiz cannot have more than {MaxQuestions} questions.");

            _questions.Add(question ?? throw new QuizException("Question cannot be null."));
        }

        public void Archive()
        {
            Status = QuizStatus.Archived;
        }
    }
}