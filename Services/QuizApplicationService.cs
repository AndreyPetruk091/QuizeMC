using Domain.Entities;
using Models.Quiz;
using Repositories.Abstractions;
using Services.Abstactions;
using AutoMapper;

namespace Services
{
    public class QuizApplicationService(
        IQuizRepository quizRepository,
        IMapper mapper) : IQuizApplicationService
    {
        public async Task<QuizModel?> GetQuizByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetByIdAsync(id, cancellationToken);
            return quiz == null ? null : mapper.Map<QuizModel>(quiz);
        }

        public async Task<IEnumerable<QuizModel>> GetActiveQuizzesAsync(CancellationToken cancellationToken)
        {
            var quizzes = await quizRepository.GetActiveQuizzesAsync(cancellationToken); //++
            return mapper.Map<IEnumerable<QuizModel>>(quizzes);
        }

        public async Task<QuizModel?> CreateQuizAsync(QuizModelCreate quizInfo, CancellationToken cancellationToken)
        {
            var quiz = new Quiz(quizInfo.Title);
            var createdQuiz = await quizRepository.AddAsync(quiz, cancellationToken); // 
            return createdQuiz == null ? null : mapper.Map<QuizModel>(createdQuiz);
        }

        public async Task<bool> UpdateQuizAsync(QuizModel quiz, CancellationToken cancellationToken)
        {
            var entity = await quizRepository.GetByIdAsync(quiz.Id, cancellationToken);
            if (entity == null) return false;

            entity = mapper.Map<Quiz>(quiz);
            return await quizRepository.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteQuizAsync(Guid id, CancellationToken cancellationToken)
        {
            var quiz = await quizRepository.GetByIdAsync(id, cancellationToken);
            return quiz != null && await quizRepository.DeleteAsync(quiz, cancellationToken);
        }
    }
}