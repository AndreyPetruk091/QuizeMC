using QuizeMC.Domain.Entities;

namespace QuizeMC.Domain.Repositories.Abstractions
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Task<Admin> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<Admin> GetWithQuizzesAsync(Guid adminId);
        Task<Admin> GetWithCategoriesAsync(Guid adminId);
    }
}