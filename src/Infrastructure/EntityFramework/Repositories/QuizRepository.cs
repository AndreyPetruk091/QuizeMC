using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Infrastructure.EntityFramework;
using QuizeMC.Domain.Enums;

namespace QuizeMC.Infrastructure.EntityFramework.Repositories
{
    public class QuizRepository : BaseRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Quiz?> GetWithQuestionsAndAnswersAsync(Guid quizId)
        {
            return await _dbSet
                .Include(q => q.Questions)
                    .ThenInclude(question => question.Answers)
                .Include(q => q.Category)
                .Include(q => q.CreatedByAdmin)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<Quiz?> GetWithCategoryAsync(Guid quizId)
        {
            return await _dbSet
                .Include(q => q.Category)
                .Include(q => q.CreatedByAdmin)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<Quiz?> GetWithAdminAsync(Guid quizId)
        {
            return await _dbSet
                .Include(q => q.CreatedByAdmin)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<IEnumerable<Quiz>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(q => q.Category)
                .Include(q => q.CreatedByAdmin)
                .Where(q => q.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetByAdminIdAsync(Guid adminId)
        {
            return await _dbSet
                .Include(q => q.Category)
                .Where(q => q.CreatedByAdminId == adminId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetByStatusAsync(QuizStatus status)
        {
            return await _dbSet
                .Include(q => q.Category)
                .Include(q => q.CreatedByAdmin)
                .Where(q => q.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quiz>> GetByAdminAndStatusAsync(Guid adminId, QuizStatus status)
        {
            return await _dbSet
                .Include(q => q.Category)
                .Where(q => q.CreatedByAdminId == adminId && q.Status == status)
                .ToListAsync();
        }

        public async Task<bool> TitleExistsAsync(string title)
        {
            return await _dbSet
                .AnyAsync(q => q.Title.Value == title);
        }

        public async Task<int> GetQuizzesCountByAdminAsync(Guid adminId)
        {
            return await _dbSet
                .CountAsync(q => q.CreatedByAdminId == adminId);
        }

        public async Task<int> GetQuizzesCountByCategoryAsync(Guid categoryId)
        {
            return await _dbSet
                .CountAsync(q => q.CategoryId == categoryId);
        }
    }
}