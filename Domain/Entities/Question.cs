using Domain.Entities.Base;
using ValueObjects;

namespace Domain.Entities
{
    public class Question : EntityBase
    {
        private readonly HashSet<Answer> _answers = new();
        public QuestionText Text { get; private set; }
        public IEnumerable<Answer> Answers => _answers.AsEnumerable();
        public AnswerIndex CorrectAnswerIndex { get; private set; }


        public Question(QuestionText text, IEnumerable<Answer> answers, AnswerIndex correctAnswerIndex)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));

            if (answers == null || !answers.Any())
                throw new ArgumentException("Вопрос должен содержать хотя бы один ответ", nameof(answers));

            foreach (var answer in answers)
            {
                _answers.Add(answer);
            }

            ValidateCorrectAnswerIndex(correctAnswerIndex);
            CorrectAnswerIndex = correctAnswerIndex;
        }

        // Добавление ответа
        public void AddAnswer(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            _answers.Add(answer);
        }

        // Удаление ответа (с проверкой корректности индекса)
        public void RemoveAnswer(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (!_answers.Contains(answer))
                return;

            var answerList = _answers.ToList();
            int index = answerList.IndexOf(answer);

            _answers.Remove(answer);

            // Если удалили ответ перед правильным индексом, корректируем его
            if (index < CorrectAnswerIndex.Value)
            {
                var newIndex = CorrectAnswerIndex.Value - 1;
                ValidateCorrectAnswerIndex(new AnswerIndex(newIndex));
                CorrectAnswerIndex = new AnswerIndex(newIndex);
            }
        }

        // Обновление правильного индекса
        public void UpdateCorrectAnswerIndex(AnswerIndex newIndex)
        {
            ValidateCorrectAnswerIndex(newIndex);
            CorrectAnswerIndex = newIndex;
        }

        private void ValidateCorrectAnswerIndex(AnswerIndex index)
        {
            if (index.Value < 0 || index.Value >= _answers.Count)
                throw new ArgumentOutOfRangeException("Индекс правильного ответа вне диапазона");
        }

    }
}
