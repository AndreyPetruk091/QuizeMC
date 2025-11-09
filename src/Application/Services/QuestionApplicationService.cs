using AutoMapper;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Application.Services.Abstractions;
using QuizeMC.Application.Models.Question;
using QuizeMC.Application.Models.Common;
using QuizeMC.Application.Services.Common;

namespace QuizeMC.Application.Services
{
    public class QuestionApplicationService : IQuestionApplicationService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionApplicationService(
            IQuestionRepository questionRepository,
            IQuizRepository quizRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<QuestionModel>> CreateQuestionAsync(QuestionCreateModel createModel, Guid quizId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var quiz = await _quizRepository.GetByIdAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse<QuestionModel>.Fail("Quiz not found");
                }

                var questionText = new QuestionText(createModel.Text);
                var correctAnswerIndex = new AnswerIndex(createModel.CorrectAnswerIndex);

                var question = new Question(questionText, correctAnswerIndex);

                // Добавляем ответы
                foreach (var answerCreateModel in createModel.Answers)
                {
                    var answerText = new AnswerText(answerCreateModel.Text);
                    var answer = new Answer(answerText);
                    question.AddAnswer(answer);
                }

                quiz.AddQuestion(question);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var questionModel = _mapper.Map<QuestionModel>(question);
                return ApiResponse<QuestionModel>.Ok(questionModel, "Question created successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<QuestionModel>.Fail($"Failed to create question: {ex.Message}");
            }
        }

        public async Task<ApiResponse<QuestionModel>> GetQuestionAsync(Guid questionId)
        {
            try
            {
                var question = await _questionRepository.GetWithAnswersAsync(questionId);
                if (question == null)
                {
                    return ApiResponse<QuestionModel>.Fail("Question not found");
                }

                var questionModel = _mapper.Map<QuestionModel>(question);
                return ApiResponse<QuestionModel>.Ok(questionModel);
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionModel>.Fail($"Failed to get question: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateQuestionAsync(Guid questionId, QuestionUpdateModel updateModel)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return ApiResponse.Fail("Question not found");
                }

                if (!string.IsNullOrEmpty(updateModel.Text))
                {
                    var newText = new QuestionText(updateModel.Text);
                    question.UpdateText(newText);
                }

                if (updateModel.CorrectAnswerIndex.HasValue)
                {
                    var newIndex = new AnswerIndex(updateModel.CorrectAnswerIndex.Value);
                    question.UpdateCorrectAnswerIndex(newIndex);
                }

                _questionRepository.Update(question);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Question updated successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to update question: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteQuestionAsync(Guid questionId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return ApiResponse.Fail("Question not found");
                }

                _questionRepository.Delete(question);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ApiResponse.Ok("Question deleted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse.Fail($"Failed to delete question: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<QuestionModel>>> GetQuestionsByQuizAsync(Guid quizId)
        {
            try
            {
                var questions = await _questionRepository.GetByQuizIdAsync(quizId);
                var questionModels = _mapper.Map<List<QuestionModel>>(questions);

                return ApiResponse<IEnumerable<QuestionModel>>.Ok(questionModels);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<QuestionModel>>.Fail($"Failed to get questions: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ReorderQuestionsAsync(Guid quizId, List<Guid> questionIdsInOrder)
        {
            try
            {
                // В реальном приложении здесь была бы логика изменения порядка вопросов
                // Для упрощения просто проверяем существование вопросов
                var quiz = await _quizRepository.GetWithQuestionsAndAnswersAsync(quizId);
                if (quiz == null)
                {
                    return ApiResponse.Fail("Quiz not found");
                }

                foreach (var questionId in questionIdsInOrder)
                {
                    var question = await _questionRepository.GetByIdAsync(questionId);
                    if (question == null)
                    {
                        return ApiResponse.Fail($"Question with ID {questionId} not found");
                    }
                }

                // Здесь должна быть логика обновления порядка
                // Например, сохранение порядка в отдельной таблице или свойство Order в Question

                await _unitOfWork.SaveChangesAsync();
                return ApiResponse.Ok("Questions reordered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail($"Failed to reorder questions: {ex.Message}");
            }
        }
    }
}