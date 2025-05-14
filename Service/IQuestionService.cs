using Domain.Entities;
using Domain.Exceptions;
using Repositories.Abstractions;
using ValueObjects;
using ValueObjects.Exceptions;

namespace Service
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestion _questionRepository;
        private readonly IQuiz _quizRepository;

        public QuestionService(IQuestion questionRepository, IQuiz quizRepository)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
        }

        public async Task<Question> CreateAsync(
            QuestionText text,
            List<Answer> answers,
            AnswerIndex correctIndex,
            CancellationToken ct)
        {
            // Валидация
            if (answers == null || answers.Count == 0)
                throw new QuestionException("Question must have at least one answer");

            if (correctIndex.Value >= answers.Count)
                throw new AnswerIndexException("Correct index out of range");

            var question = new Question(text, answers, correctIndex);
            return await _questionRepository.AddAsync(question, ct)
                ?? throw new DomainException("Failed to create question");
        }

        public async Task AddAnswerAsync(Guid questionId, Answer answer, CancellationToken ct)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, ct)
                ?? throw new QuestionException("Question not found");

            question.AddAnswer(answer ?? throw new AnswerException("Answer cannot be null"));
            await _questionRepository.UpdateAsync(question, ct);
        }

        public async Task RemoveAnswerAsync(Guid questionId, Answer answer, CancellationToken ct)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, ct)
                ?? throw new QuestionException("Question not found");

            question.RemoveAnswer(answer ?? throw new AnswerException("Answer cannot be null"));
            await _questionRepository.UpdateAsync(question, ct);
        }

        public async Task SetCorrectAnswerAsync(Guid questionId, AnswerIndex newIndex, CancellationToken ct)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, ct)
                ?? throw new QuestionException("Question not found");

            if (newIndex.Value >= question.Answers.Count)
                throw new AnswerIndexException("Index out of range");

            question.UpdateCorrectAnswerIndex(newIndex);
            await _questionRepository.UpdateAsync(question, ct);
        }

        // Реализация IService<T>
        public Task<Question?> GetByIdAsync(Guid id, CancellationToken ct)
            => _questionRepository.GetByIdAsync(id, ct);

        public Task<List<Question>> GetAllAsync(CancellationToken ct)
            => _questionRepository.GetAllAsync(ct);

        public async Task<List<Question>> GetByQuizIdAsync(Guid quizId, CancellationToken ct)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId, ct);
            return quiz?.Questions.ToList() ?? new List<Question>();
        }
    }
}