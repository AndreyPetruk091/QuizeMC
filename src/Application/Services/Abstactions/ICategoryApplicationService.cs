using QuizeMC.Application.Models.Category;
using QuizeMC.Application.Models.Common;

namespace QuizeMC.Application.Services.Abstractions
{
    public interface ICategoryApplicationService
    {
        Task<ApiResponse<CategoryModel>> CreateCategoryAsync(CategoryCreateModel createModel, Guid adminId);
        Task<ApiResponse<CategoryModel>> GetCategoryAsync(Guid categoryId);
        Task<ApiResponse> UpdateCategoryAsync(Guid categoryId, CategoryUpdateModel updateModel);
        Task<ApiResponse> DeleteCategoryAsync(Guid categoryId);
        Task<ApiResponse<PagedResult<CategoryModel>>> GetCategoriesPagedAsync(PagedRequest request);
        Task<ApiResponse<IEnumerable<CategoryModel>>> GetCategoriesByAdminAsync(Guid adminId);
    }
}