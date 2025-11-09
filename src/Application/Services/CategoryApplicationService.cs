using AutoMapper;
using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Application.Services.Abstractions;
using QuizeMC.Application.Models.Category;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Common;

namespace QuizeMC.Application.Services
{
    public class CategoryApplicationService : ICategoryApplicationService
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryApplicationService(
            ICategoryService categoryService,
            ICategoryRepository categoryRepository,
            IAdminRepository adminRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _adminRepository = adminRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CategoryModel>> CreateCategoryAsync(CategoryCreateModel createModel, Guid adminId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse<CategoryModel>.Fail("Admin not found");
                }

                var categoryName = new CategoryName(createModel.Name);
                var categoryDescription = new CategoryDescription(createModel.Description);

                var category = await _categoryService.CreateCategoryAsync(categoryName, categoryDescription, admin);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var categoryModel = _mapper.Map<CategoryModel>(category);
                return ApiResponse<CategoryModel>.Ok(categoryModel, "Category created successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<CategoryModel>.Fail($"Failed to create category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryModel>> GetCategoryAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetWithQuizzesAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse<CategoryModel>.Fail("Category not found");
                }

                var categoryModel = _mapper.Map<CategoryModel>(category);
                return ApiResponse<CategoryModel>.Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryModel>.Fail($"Failed to get category: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateCategoryAsync(Guid categoryId, CategoryUpdateModel updateModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse.Fail("Category not found");
                }

                if (!string.IsNullOrEmpty(updateModel.Name))
                {
                    var newName = new CategoryName(updateModel.Name);
                    category.UpdateName(newName);
                }

                if (!string.IsNullOrEmpty(updateModel.Description))
                {
                    var newDescription = new CategoryDescription(updateModel.Description);
                    category.UpdateDescription(newDescription);
                }

                _categoryRepository.Update(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Category updated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to update category: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse.Fail("Category not found");
                }

                var canDelete = await _categoryService.CanDeleteCategoryAsync(category);
                if (!canDelete)
                {
                    return ApiResponse.Fail("Cannot delete category that contains quizzes");
                }

                _categoryRepository.Delete(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Category deleted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to delete category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResult<CategoryModel>>> GetCategoriesPagedAsync(PagedRequest request)
        {
            try
            {
                var categories = await _categoryRepository.GetAllWithQuizzesCountAsync();
                var categoriesList = categories.ToList();

                // Применяем поиск
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    categoriesList = categoriesList
                        .Where(c => c.Name.Value.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   c.Description.Value.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Применяем сортировку
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    categoriesList = request.SortBy.ToLower() switch
                    {
                        "name" => request.SortDescending
                            ? categoriesList.OrderByDescending(c => c.Name.Value).ToList()
                            : categoriesList.OrderBy(c => c.Name.Value).ToList(),
                        "quizzescount" => request.SortDescending
                            ? categoriesList.OrderByDescending(c => c.Quizzes.Count).ToList()
                            : categoriesList.OrderBy(c => c.Quizzes.Count).ToList(),
                        "createdat" => request.SortDescending
                            ? categoriesList.OrderByDescending(c => c.CreatedAt).ToList()
                            : categoriesList.OrderBy(c => c.CreatedAt).ToList(),
                        _ => categoriesList
                    };
                }

                // Применяем пагинацию
                var totalCount = categoriesList.Count;
                var pagedCategories = categoriesList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var categoryModels = _mapper.Map<List<CategoryModel>>(pagedCategories);
                var result = new PagedResult<CategoryModel>(
                    categoryModels,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return ApiResponse<PagedResult<CategoryModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<CategoryModel>>.Fail($"Failed to get categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryModel>>> GetCategoriesByAdminAsync(Guid adminId)
        {
            try
            {
                var categories = await _categoryRepository.GetByAdminIdAsync(adminId);
                var categoryModels = _mapper.Map<List<CategoryModel>>(categories);

                return ApiResponse<IEnumerable<CategoryModel>>.Ok(categoryModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<CategoryModel>>.Fail($"Failed to get categories: {ex.Message}");
            }
        }
    }
}