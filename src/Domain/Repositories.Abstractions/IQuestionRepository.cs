using QuizeMC.Domain.Entities;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question> GetWithAnswersAsync(Guid questionId);
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId);
        Task<int> GetQuestionsCountByQuizAsync(Guid quizId);
        Task<bool> QuestionExistsInQuizAsync(Guid quizId, string questionText);
    }
}