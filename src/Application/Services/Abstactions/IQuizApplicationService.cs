
using QuizeMC.Application.Application.Models.Quiz;
using QuizeMC.Application.Models.Quiz;


namespace QuizeMC.Application.Services.Abstactions
{
    public interface IQuizApplicationService
    {
        Task<QuizModel?> GetQuizByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<QuizModel>> GetActiveQuizzesAsync(int skip, int take, CancellationToken ct);
        Task<QuizModel?> CreateQuizAsync(CreateQuizModel model, CancellationToken ct);
        Task<bool> UpdateQuizAsync(QuizModel model, CancellationToken ct);
        Task<bool> DeleteQuizAsync(Guid id, CancellationToken ct);
    }
}