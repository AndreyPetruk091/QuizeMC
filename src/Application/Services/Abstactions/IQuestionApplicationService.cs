using QuizeMC.Application.Models.Question;
using QuizeMC.Application.Models.Common;

namespace QuizeMC.Application.Services.Abstractions
{
    public interface IQuestionApplicationService
    {
        Task<ApiResponse<QuestionModel>> CreateQuestionAsync(QuestionCreateModel createModel, Guid quizId);
        Task<ApiResponse<QuestionModel>> GetQuestionAsync(Guid questionId);
        Task<ApiResponse> UpdateQuestionAsync(Guid questionId, QuestionUpdateModel updateModel);
        Task<ApiResponse> DeleteQuestionAsync(Guid questionId);
        Task<ApiResponse<IEnumerable<QuestionModel>>> GetQuestionsByQuizAsync(Guid quizId);
        Task<ApiResponse> ReorderQuestionsAsync(Guid quizId, List<Guid> questionIdsInOrder);
    }
}