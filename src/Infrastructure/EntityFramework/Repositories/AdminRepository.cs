using Microsoft.EntityFrameworkCore;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Infrastructure.EntityFramework;

namespace QuizeMC.Infrastructure.EntityFramework.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.Email.Value == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(a => a.Email.Value == email);
        }

        public async Task<Admin?> GetWithQuizzesAsync(Guid adminId)
        {
            return await _dbSet
                .Include(a => a.CreatedQuizzes)
                    .ThenInclude(q => q.Category)
                .Include(a => a.CreatedQuizzes)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(a => a.Id == adminId);
        }

        public async Task<Admin?> GetWithCategoriesAsync(Guid adminId)
        {
            return await _dbSet
                .Include(a => a.CreatedCategories)
                .FirstOrDefaultAsync(a => a.Id == adminId);
        }
    }
}