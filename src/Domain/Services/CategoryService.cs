using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;

namespace QuizeMC.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuizRepository _quizRepository;

        public CategoryService(ICategoryRepository categoryRepository, IQuizRepository quizRepository)
        {
            _categoryRepository = categoryRepository;
            _quizRepository = quizRepository;
        }

        public async Task<Category> CreateCategoryAsync(CategoryName name, CategoryDescription description, Admin createdByAdmin)
        {
            // Проверяем, существует ли категория с таким именем
            var existingCategory = await _categoryRepository.GetByNameAsync(name.Value);
            if (existingCategory != null)
                throw new CategoryException($"Category with name '{name.Value}' already exists");

            // Создаем категорию
            var category = new Category(name, description, createdByAdmin);

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<bool> CanDeleteCategoryAsync(Category category)
        {
            // Проверяем, есть ли викторины в этой категории
            var quizzesInCategory = await _quizRepository.GetByCategoryIdAsync(category.Id);
            return !quizzesInCategory.Any();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithStatsAsync()
        {
            var categories = await _categoryRepository.GetAllWithQuizzesCountAsync();
            return categories;
        }
    }
}