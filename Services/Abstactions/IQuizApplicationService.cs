
using Models.Quiz;

namespace Services.Abstactions
{
    public interface IQuizApplicationService
    {
        Task<QuizModel?> GetQuizByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<QuizModel>> GetActiveQuizzesAsync(CancellationToken cancellationToken);
        Task<QuizModel?> CreateQuizAsync(QuizModelCreate quizInfo, CancellationToken cancellationToken);
        Task<bool> UpdateQuizAsync(QuizModel quiz, CancellationToken cancellationToken);
        Task<bool> DeleteQuizAsync(Guid id, CancellationToken cancellationToken);
    }
}
