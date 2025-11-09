using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;

namespace QuizeMC.Domain.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CategoryName name, CategoryDescription description, Admin createdByAdmin);
        Task<bool> CanDeleteCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetCategoriesWithStatsAsync();
    }
}