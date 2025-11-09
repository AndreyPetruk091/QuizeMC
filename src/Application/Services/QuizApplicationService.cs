using AutoMapper;
using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Application.Services.Abstractions;
using QuizeMC.Application.Models.Quiz;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Common;

namespace QuizeMC.Application.Services
{
    public class QuizApplicationService : IQuizApplicationService
    {
        private readonly IQuizService _quizService;
        private readonly IQuizRepository _quizRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QuizApplicationService(
            IQuizService quizService,
            IQuizRepository quizRepository,
            ICategoryRepository categoryRepository,
            IAdminRepository adminRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _quizService = quizService;
            _quizRepository = quizRepository;
            _categoryRepository = categoryRepository;
            _adminRepository = adminRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<QuizModel>> CreateQuizAsync(QuizCreateModel createModel, Guid adminId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse<QuizModel>.Fail("Admin not found");
                }

                var category = await _categoryRepository.GetByIdAsync(createModel.CategoryId);
                if (category == null)
                {
                    return ApiResponse<QuizModel>.Fail("Category not found");
                }

                var quizTitle = new QuizTitle(createModel.Title);
                var quizDescription = new QuizDescription(createModel.Description);

                var quiz = await _quizService.CreateQuizAsync(quizTitle, quizDescription, category, admin);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var quizModel = _mapper.Map<QuizModel>(quiz);
                return ApiResponse<QuizModel>.Ok(quizModel, "Quiz created successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<QuizModel>.Fail($"Failed to create quiz: {ex.Message}");
            }
        }

        public async Task<ApiResponse<QuizModel>> GetQuizAsync(Guid quizId)
        {
            try
            {
                var quiz = await _quizRepository.GetWithCategoryAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse<QuizModel>.Fail("Quiz not found");
                }

                var quizModel = _mapper.Map<QuizModel>(quiz);
                return ApiResponse<QuizModel>.Ok(quizModel);
            }
            catch (Exception ex)
            {
                return ApiResponse<QuizModel>.Fail($"Failed to get quiz: {ex.Message}");
            }
        }

        public async Task<ApiResponse<QuizModel>> GetQuizWithDetailsAsync(Guid quizId)
        {
            try
            {
                var quiz = await _quizRepository.GetWithQuestionsAndAnswersAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse<QuizModel>.Fail("Quiz not found");
                }

                var quizModel = _mapper.Map<QuizModel>(quiz);
                return ApiResponse<QuizModel>.Ok(quizModel);
            }
            catch (Exception ex)
            {
                return ApiResponse<QuizModel>.Fail($"Failed to get quiz details: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateQuizAsync(Guid quizId, QuizUpdateModel updateModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse.Fail("Quiz not found");
                }

                if (!string.IsNullOrEmpty(updateModel.Title))
                {
                    var newTitle = new QuizTitle(updateModel.Title);
                    quiz.UpdateTitle(newTitle);
                }

                if (!string.IsNullOrEmpty(updateModel.Description))
                {
                    var newDescription = new QuizDescription(updateModel.Description);
                    quiz.UpdateDescription(newDescription);
                }

                if (updateModel.CategoryId.HasValue)
                {
                    var category = await _categoryRepository.GetByIdAsync(updateModel.CategoryId.Value);
                    if (category == null)
                    {
                        return ApiResponse.Fail("Category not found");
                    }
                    quiz.UpdateCategory(category);
                }

                _quizRepository.Update(quiz);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Quiz updated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to update quiz: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateQuizStatusAsync(Guid quizId, QuizStatusUpdateModel updateModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse.Fail("Quiz not found");
                }

                switch (updateModel.Status)
                {
                    case Domain.Enums.QuizStatus.Active:
                        var canPublish = await _quizService.CanPublishQuizAsync(quiz);
                        if (!canPublish)
                        {
                            return ApiResponse.Fail("Cannot publish quiz. Ensure it has questions and all questions have valid answers.");
                        }
                        quiz.Publish();
                        break;

                    case Domain.Enums.QuizStatus.Archived:
                        var canArchive = await _quizService.CanArchiveQuizAsync(quiz);
                        if (!canArchive)
                        {
                            return ApiResponse.Fail("Cannot archive quiz.");
                        }
                        quiz.Archive();
                        break;

                    case Domain.Enums.QuizStatus.Draft:
                        quiz.MoveToDraft();
                        break;

                    default:
                        return ApiResponse.Fail("Invalid quiz status");
                }

                _quizRepository.Update(quiz);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok($"Quiz status updated to {updateModel.Status}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to update quiz status: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteQuizAsync(Guid quizId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse.Fail("Quiz not found");
                }

                _quizRepository.Delete(quiz);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Quiz deleted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to delete quiz: {ex.Message}");
            }
        }

        public async Task<ApiResponse<QuizModel>> DuplicateQuizAsync(Guid quizId, Guid adminId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var originalQuiz = await _quizRepository.GetWithQuestionsAndAnswersAsync(quizId);
                if (originalQuiz == null)
                {
                    return ApiResponse<QuizModel>.Fail("Original quiz not found");
                }

                var admin = await _adminRepository.GetByIdAsync(adminId);
                if (admin == null)
                {
                    return ApiResponse<QuizModel>.Fail("Admin not found");
                }

                var duplicatedQuiz = await _quizService.DuplicateQuizAsync(originalQuiz, admin);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var quizModel = _mapper.Map<QuizModel>(duplicatedQuiz);
                return ApiResponse<QuizModel>.Ok(quizModel, "Quiz duplicated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<QuizModel>.Fail($"Failed to duplicate quiz: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesPagedAsync(PagedRequest request)
        {
            try
            {
                var quizzes = await _quizRepository.GetAllAsync();
                var quizzesList = quizzes.ToList();

                // Применяем поиск и фильтрацию
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    quizzesList = quizzesList
                        .Where(q => q.Title.Value.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   q.Description.Value.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Применяем сортировку
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    quizzesList = request.SortBy.ToLower() switch
                    {
                        "title" => request.SortDescending
                            ? quizzesList.OrderByDescending(q => q.Title.Value).ToList()
                            : quizzesList.OrderBy(q => q.Title.Value).ToList(),
                        "status" => request.SortDescending
                            ? quizzesList.OrderByDescending(q => q.Status).ToList()
                            : quizzesList.OrderBy(q => q.Status).ToList(),
                        "createdat" => request.SortDescending
                            ? quizzesList.OrderByDescending(q => q.CreatedAt).ToList()
                            : quizzesList.OrderBy(q => q.CreatedAt).ToList(),
                        _ => quizzesList
                    };
                }

                // Применяем пагинацию
                var totalCount = quizzesList.Count;
                var pagedQuizzes = quizzesList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var quizModels = _mapper.Map<List<QuizModel>>(pagedQuizzes);
                var result = new PagedResult<QuizModel>(
                    quizModels,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return ApiResponse<PagedResult<QuizModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<QuizModel>>.Fail($"Failed to get quizzes: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesByAdminAsync(Guid adminId, PagedRequest request)
        {
            try
            {
                var quizzes = await _quizRepository.GetByAdminIdAsync(adminId);
                var quizzesList = quizzes.ToList();

                // Применяем пагинацию
                var totalCount = quizzesList.Count;
                var pagedQuizzes = quizzesList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var quizModels = _mapper.Map<List<QuizModel>>(pagedQuizzes);
                var result = new PagedResult<QuizModel>(
                    quizModels,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return ApiResponse<PagedResult<QuizModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<QuizModel>>.Fail($"Failed to get quizzes: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PagedResult<QuizModel>>> GetQuizzesByCategoryAsync(Guid categoryId, PagedRequest request)
        {
            try
            {
                var quizzes = await _quizRepository.GetByCategoryIdAsync(categoryId);
                var quizzesList = quizzes.ToList();

                // Применяем пагинацию
                var totalCount = quizzesList.Count;
                var pagedQuizzes = quizzesList
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var quizModels = _mapper.Map<List<QuizModel>>(pagedQuizzes);
                var result = new PagedResult<QuizModel>(
                    quizModels,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return ApiResponse<PagedResult<QuizModel>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<QuizModel>>.Fail($"Failed to get quizzes: {ex.Message}");
            }
        }

        public async Task<ApiResponse<int>> CalculateQuizComplexityAsync(Guid quizId)
        {
            try
            {
                var quiz = await _quizRepository.GetWithQuestionsAndAnswersAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse<int>.Fail("Quiz not found");
                }

                var complexity = await _quizService.CalculateQuizComplexityAsync(quiz);
                return ApiResponse<int>.Ok(complexity);
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"Failed to calculate quiz complexity: {ex.Message}");
            }
        }
    }
}