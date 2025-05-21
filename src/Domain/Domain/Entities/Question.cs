using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Entities.Base;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Entities
{
    public class Question : EntityBase
    {
        private readonly List<Answer> _answers = new();
        public virtual IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();
        public QuestionText Text { get; private set; }
        public AnswerIndex CorrectAnswerIndex { get; private set; }

        // Конструктор для EF Core
        protected Question() { }

        public Question(QuestionText text, AnswerIndex correctAnswerIndex)
        {
            Text = text ?? throw new QuestionException("Question text cannot be null.");
            CorrectAnswerIndex = correctAnswerIndex;
        }

        public void AddAnswers(IEnumerable<Answer> answers)
        {
            foreach (var answer in answers)
            {
                AddAnswer(answer);
            }
        }

        public void AddAnswer(Answer answer)
        {
            if (answer == null)
                throw new QuestionException("Answer cannot be null.");

            _answers.Add(answer);
            ValidateAnswerIndex(CorrectAnswerIndex);
        }

        public void RemoveAnswer(Answer answer)
        {
            if (!_answers.Contains(answer)) return;

            _answers.Remove(answer);
            ValidateAnswerIndex(CorrectAnswerIndex);
        }

        private void ValidateAnswerIndex(AnswerIndex index)
        {
            if (index.Value < 0 || index.Value >= _answers.Count)
                throw new QuestionException($"Answer index must be between 0 and {_answers.Count - 1}.");
        }
    }
}