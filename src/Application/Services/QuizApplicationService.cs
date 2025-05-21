
using AutoMapper;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Application.Models.Quiz;
using QuizeMC.Application.Application.Models.Quiz;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Entities;
using QuizeMC.Application.Services.Abstactions;

namespace Services.Implementations
{
    public sealed class QuizApplicationService : IQuizApplicationService
    {
        private readonly IQuiz _quizRepository;
        private readonly IQuestion _questionRepository;
        private readonly IMapper _mapper;

        public QuizApplicationService(
            IQuiz quizRepository,
            IQuestion questionRepository,
            IMapper mapper)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<QuizModel?> GetQuizByIdAsync(Guid id, CancellationToken ct)
        {
            var quiz = await _quizRepository.GetByIdAsync(id, ct);
            return quiz == null ? null : _mapper.Map<QuizModel>(quiz);
        }

        public async Task<IEnumerable<QuizModel>> GetActiveQuizzesAsync(int skip, int take, CancellationToken ct)
        {
            var quizzes = await _quizRepository.GetActiveAsync(skip, take, ct);
            return _mapper.Map<IEnumerable<QuizModel>>(quizzes);
        }

        public async Task<QuizModel?> CreateQuizAsync(CreateQuizModel model, CancellationToken ct)
        {
            var quizTitle = new QuizTitle(model.Title);
            if (await _quizRepository.ExistsByTitleAsync(quizTitle.Value, ct))
                return null;

            var quiz = new Quiz(quizTitle);
            var createdQuiz = await _quizRepository.AddAsync(quiz, ct);
            return createdQuiz == null ? null : _mapper.Map<QuizModel>(createdQuiz);
        }

        public async Task<bool> UpdateQuizAsync(QuizModel model, CancellationToken ct)
        {
            var existingQuiz = await _quizRepository.GetByIdAsync(model.Id, ct);
            if (existingQuiz == null) return false;

            _mapper.Map(model, existingQuiz);
            return await _quizRepository.UpdateAsync(existingQuiz, ct);
        }

        public async Task<bool> DeleteQuizAsync(Guid id, CancellationToken ct)
        {
            var quiz = await _quizRepository.GetByIdAsync(id, ct);
            return quiz != null && await _quizRepository.DeleteAsync(quiz, ct);
        }

        public async Task<bool> PublishQuizAsync(Guid quizId, CancellationToken ct)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId, ct);
            if (quiz == null || !quiz.Questions.Any()) return false;

            quiz.Publish();
            return await _quizRepository.UpdateAsync(quiz, ct);
        }

        public Task<QuizModel?> GetQuizModel(CreateQuizModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}