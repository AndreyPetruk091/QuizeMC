using Models.Question;


namespace Services.Abstractions
{
    public interface IQuestionApplicationService
    {
        Task<QuestionModel?> GetQuestionByIdAsync(Guid id, CancellationToken ct);
        Task<bool> AddQuestionToQuizAsync(Guid quizId, QuestionCreateModel model, CancellationToken ct);
        Task<bool> RemoveQuestionFromQuizAsync(Guid questionId, CancellationToken ct);
    }
}