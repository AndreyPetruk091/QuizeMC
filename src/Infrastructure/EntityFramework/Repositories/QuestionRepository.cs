using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Infrastructure.EntityFramework;

namespace QuizeMC.Infrastructure.EntityFramework.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Question?> GetWithAnswersAsync(Guid questionId)
        {
            return await _dbSet
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId)
        {
            return await _dbSet
                .Include(q => q.Answers)
                .Where(q => EF.Property<Guid>(q, "QuizId") == quizId)
                .ToListAsync();
        }

        public async Task<int> GetQuestionsCountByQuizAsync(Guid quizId)
        {
            return await _dbSet
                .CountAsync(q => EF.Property<Guid>(q, "QuizId") == quizId);
        }

        public async Task<bool> QuestionExistsInQuizAsync(Guid quizId, string questionText)
        {
            return await _dbSet
                .AnyAsync(q => EF.Property<Guid>(q, "QuizId") == quizId && q.Text.Value == questionText);
        }
    }
}