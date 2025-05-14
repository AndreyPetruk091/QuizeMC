using Domain.Entities.Base;
using Domain.Exceptions;

using ValueObjects;

namespace Domain.Entities
{
    public class Question : EntityBase
    {
        private readonly List<Answer> _answers = new();
        public QuestionText Text { get; private set; }
        public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();
        public AnswerIndex CorrectAnswerIndex { get; private set; }

        public Question(QuestionText text, IEnumerable<Answer> answers, AnswerIndex correctAnswerIndex)
        {
            Text = text ?? throw new QuestionException("Question text cannot be null.");
            _answers = answers?.ToList() ?? throw new QuestionException("Answers list cannot be null.");

            if (_answers.Count == 0)
                throw new QuestionException("Question must have at least one answer.");

            ValidateAnswerIndex(correctAnswerIndex);
            CorrectAnswerIndex = correctAnswerIndex;
        }

        private void ValidateAnswerIndex(AnswerIndex index)
        {
            if (index.Value < 0 || index.Value >= _answers.Count)
                throw new QuestionException($"Answer index must be between 0 and {_answers.Count - 1}.");
        }

        public void AddAnswer(Answer answer)
        {
            _answers.Add(answer ?? throw new QuestionException("Answer cannot be null."));
        }

        public void RemoveAnswer(Answer answer)
        {
            if (answer == null || !_answers.Contains(answer))
                return;

            _answers.Remove(answer);
        }
    }
}