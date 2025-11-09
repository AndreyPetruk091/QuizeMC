using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Services.Abstractions
{
    public interface IQuizService
    {
        Task<Quiz> CreateQuizAsync(QuizTitle title, QuizDescription description, Category category, Admin createdByAdmin);
        Task<bool> CanPublishQuizAsync(Quiz quiz);
        Task<bool> CanArchiveQuizAsync(Quiz quiz);
        Task<Quiz> DuplicateQuizAsync(Quiz originalQuiz, Admin newOwner);
        Task<int> CalculateQuizComplexityAsync(Quiz quiz);
    }
}