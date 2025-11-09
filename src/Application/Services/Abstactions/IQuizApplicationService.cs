using QuizeMC.Application.Models.Quiz;
using QuizeMC.Application.Models.Common;

namespace QuizeMC.Application.Services.Abstractions
{
    public interface IQuizApplicationService
    {
        Task<ApiResponse<QuizModel>> CreateQuizAsync(QuizCreateModel createModel, Guid adminId);
        Task<ApiResponse<QuizModel>> GetQuizAsync(Guid quizId);
        Task<ApiResponse<QuizModel>> GetQuizWithDetailsAsync(Guid quizId);
        Task<ApiResponse> UpdateQuizAsync(Guid quizId, QuizUpdateModel updateModel);
        Task<ApiResponse> UpdateQuizStatusAsync(Guid quizId, QuizStatusUpdateModel updateModel);
        Task<ApiResponse> DeleteQuizAsync(Guid quizId);
        Task<ApiResponse<QuizModel>> DuplicateQuizAsync(Guid quizId, Guid adminId);
        Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesPagedAsync(PagedRequest request);
        Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesByAdminAsync(Guid adminId, PagedRequest request);
        Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesByCategoryAsync(Guid categoryId, PagedRequest request);
        Task<ApiResponse<int>> CalculateQuizComplexityAsync(Guid quizId);
    }
}