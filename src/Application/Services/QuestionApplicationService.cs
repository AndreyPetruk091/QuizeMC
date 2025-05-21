using AutoMapper;
using QuizeMC.Application.Services.Abstactions;
using QuizeMC.Domain.Repositories.Abstractions;
using QuizeMC.Application.Models.Question;
using QuizeMC.Domain.Entities;
using QuizeMC.Domain.ValueObjects;
using QuizeMC.Domain.Exceptions;

namespace QuizeMC.Application.Services
{
    public class QuestionApplicationService : IQuestionApplicationService
    {
        private readonly IQuestion _questionRepository;
        private readonly IQuiz _quizRepository;
        private readonly IMapper _mapper;

        public QuestionApplicationService(
            IQuestion questionRepository,
            IQuiz quizRepository,
            IMapper mapper)
        {
            _questionRepository = questionRepository;
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<QuestionModel?> AddQuestionToQuizAsync(AddQuestionCommand command, CancellationToken cancellationToken)
        {
            var quiz = await _quizRepository.GetByIdAsync(command.QuizId, cancellationToken);
            if (quiz == null) return null;

            try
            {
                var question = new Question(
                    new QuestionText(command.Question.Text),
                    new AnswerIndex(command.Question.CorrectAnswerIndex));

                quiz.AddQuestion(question);
                return await _quizRepository.UpdateAsync(quiz, cancellationToken)
                    ? _mapper.Map<QuestionModel>(question)
                    : null;
            }
            catch (DomainException ex)
            {
                // Логирование ошибки при необходимости
                return null;
            }
        }

        public async Task<QuestionModel?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(id, cancellationToken);
            return question == null ? null : _mapper.Map<QuestionModel>(question);
        }

        public async Task<bool> RemoveQuestionFromQuizAsync(Guid questionId, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(questionId, cancellationToken);
            if (question == null) return false;

            var quiz = await _quizRepository.GetByIdAsync(questionId, cancellationToken);
            if (quiz != null)
            {
                quiz.AddQuestion(question);
                if (!await _quizRepository.UpdateAsync(quiz, cancellationToken))
                {
                    return false;
                }
            }

            return await _questionRepository.DeleteAsync(question, cancellationToken);
        }

        Task<bool> IQuestionApplicationService.AddQuestionToQuizAsync(Guid quizId, QuestionCreateModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<QuestionModel?> IQuestionApplicationService.GetQuestionByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<bool> IQuestionApplicationService.RemoveQuestionFromQuizAsync(Guid questionId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}