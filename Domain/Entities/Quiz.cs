using Domain.Entities.Base;
using Domain.Enums;
using Domain.Exceptions;

using ValueObjects;

namespace Domain.Entities
{
    public class Quiz : EntityBase
    {
        public QuizTitle Title { get; }
        public QuizStatus Status { get; private set; }
        private readonly List<Question> _questions = new();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        private const int MaxQuestions = 100;

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