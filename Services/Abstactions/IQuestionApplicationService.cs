using Models.Question;

namespace Services.Abstactions
{
    public interface IQuestionApplicationService
    {
        Task<QuestionModel?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> AddQuestionToQuizAsync(AddQuestionCommand command, CancellationToken cancellationToken); //++
        Task<bool> RemoveQuestionFromQuizAsync(Guid questionId, CancellationToken cancellationToken);
    }
}
