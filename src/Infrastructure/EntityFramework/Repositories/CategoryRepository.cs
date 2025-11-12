using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Infrastructure.EntityFramework;

namespace QuizeMC.Infrastructure.EntityFramework.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name.Value == name);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _dbSet
                .AnyAsync(c => c.Name.Value == name);
        }

        public async Task<Category?> GetWithQuizzesAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(c => c.Quizzes)
                    .ThenInclude(q => q.CreatedByAdmin)
                .Include(c => c.Quizzes)
                    .ThenInclude(q => q.Questions)
                .Include(c => c.CreatedByAdmin)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllWithQuizzesCountAsync()
        {
            return await _dbSet
                .Include(c => c.Quizzes)
                .Include(c => c.CreatedByAdmin)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByAdminIdAsync(Guid adminId)
        {
            return await _dbSet
                .Include(c => c.Quizzes)
                .Where(c => c.CreatedByAdminId == adminId)
                .ToListAsync();
        }
    }
}