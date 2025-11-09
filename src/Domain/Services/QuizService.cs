using QuizeMC.Domain.Services.Abstractions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;
using QuizeMC.Domain.Enums;

namespace QuizeMC.Domain.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;

        public QuizService(IQuizRepository quizRepository, IQuestionRepository questionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }

        public async Task<Quiz> CreateQuizAsync(QuizTitle title, QuizDescription description, Category category, Admin createdByAdmin)
        {
            // Проверяем, существует ли викторина с таким названием
            var existingQuiz = await _quizRepository.TitleExistsAsync(title.Value);
            if (existingQuiz)
                throw new QuizException($"Quiz with title '{title.Value}' already exists");

            // Создаем викторину
            var quiz = new Quiz(title, description, category, createdByAdmin);

            await _quizRepository.AddAsync(quiz);
            return quiz;
        }

        public async Task<bool> CanPublishQuizAsync(Quiz quiz)
        {
            // Викторина может быть опубликована, если:
            // 1. Она в статусе Draft
            // 2. Имеет хотя бы один вопрос
            // 3. Все вопросы имеют хотя бы 2 ответа
            // 4. У всех вопросов указан корректный индекс правильного ответа

            if (quiz.Status != QuizStatus.Draft)
                return false;

            if (!quiz.HasQuestions)
                return false;

            var questions = await _questionRepository.GetByQuizIdAsync(quiz.Id);

            foreach (var question in questions)
            {
                if (question.AnswersCount < 2)
                    return false;

                if (question.CorrectAnswerIndex.Value >= question.AnswersCount)
                    return false;
            }

            return true;
        }

        public Task<bool> CanArchiveQuizAsync(Quiz quiz)
        {
            // Викторина может быть архивирована из любого статуса, кроме уже архивного
            return Task.FromResult(quiz.Status != QuizStatus.Archived);
        }

        public async Task<Quiz> DuplicateQuizAsync(Quiz originalQuiz, Admin newOwner)
        {
            if (originalQuiz == null)
                throw new QuizException("Original quiz cannot be null");

            // Создаем новую викторину с приставкой "Copy"
            var newTitle = new QuizTitle($"{originalQuiz.Title.Value} (Copy)");
            var newDescription = new QuizDescription(originalQuiz.Description.Value);

            var duplicatedQuiz = new Quiz(newTitle, newDescription, originalQuiz.Category, newOwner);

            // Получаем вопросы оригинальной викторины
            var originalQuestions = await _questionRepository.GetByQuizIdAsync(originalQuiz.Id);

            foreach (var originalQuestion in originalQuestions)
            {
                // Дублируем вопрос
                var duplicatedQuestion = new Question(
                    new QuestionText(originalQuestion.Text.Value),
                    new AnswerIndex(originalQuestion.CorrectAnswerIndex.Value)
                );

                // Дублируем ответы
                foreach (var originalAnswer in originalQuestion.Answers)
                {
                    duplicatedQuestion.AddAnswer(new Answer(new AnswerText(originalAnswer.Text.Value)));
                }

                duplicatedQuiz.AddQuestion(duplicatedQuestion);
            }

            await _quizRepository.AddAsync(duplicatedQuiz);
            return duplicatedQuiz;
        }

        public async Task<int> CalculateQuizComplexityAsync(Quiz quiz)
        {
            var questions = await _questionRepository.GetByQuizIdAsync(quiz.Id);
            var questionsList = questions.ToList();

            if (!questionsList.Any())
                return 0;

            int complexity = 0;

            foreach (var question in questionsList)
            {
                // Сложность увеличивается с количеством ответов
                complexity += question.AnswersCount;

                // Дополнительная сложность за длину вопроса
                if (question.Text.Value.Length > 100)
                    complexity += 1;
            }

            // Нормализуем сложность от 1 до 10
            var questionsCount = questionsList.Count;
            var normalizedComplexity = Math.Max(1, Math.Min(10, complexity / questionsCount));

            return normalizedComplexity;
        }
    }
}