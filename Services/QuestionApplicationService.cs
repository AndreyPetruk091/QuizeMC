using AutoMapper;
using Domain.Entities;
using Models.Question;
using Repositories.Abstractions;
using Services.Abstactions;
using ValueObjects;

namespace Services
{
    public class QuestionApplicationService(
        IQuestionRepository questionRepository,
        IQuizRepository quizRepository,
        IMapper mapper) : IQuestionApplicationService
    {
        public async Task<bool> AddQuestionToQuizAsync(AddQuestionCommand command, CancellationToken cancellationToken) //++
        {
            var quiz = await quizRepository.GetByIdAsync(command.QuizId, cancellationToken);
            if (quiz == null) return false;

            var question = new Question(
                command.Question.Text,
                command.Question.Answers.Select(a => new Answer(a.Text)).ToList(),
                command.Question.CorrectAnswerIndex);

            quiz.AddQuestion(question);
            return await quizRepository.UpdateAsync(quiz, cancellationToken);
        }

        public async Task<QuestionModel?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(id, cancellationToken);
            return question == null ? null : mapper.Map<QuestionModel>(question);
        }

        public async Task<bool> RemoveQuestionFromQuizAsync(Guid questionId, CancellationToken cancellationToken)
        {
            var question = await questionRepository.GetByIdAsync(questionId, cancellationToken);
            if (question == null) return false;

            return await questionRepository.DeleteAsync(question, cancellationToken);
        }
    }
}