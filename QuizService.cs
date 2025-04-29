using Domain.Entities;
using Repositories.Abstractions;


namespace QuizeMC
{
    public class QuizService
    {
        private readonly IQuizRepository _quizRepo;
        private readonly IQuestionRepository _questionRepo;


        public QuizService(IQuizRepository quizRepo, IQuestionRepository questionRepo)
        {
            _quizRepo = quizRepo;
            _questionRepo = questionRepo;
        }

        public async Task CreateQuizAsync(Quiz quiz, CancellationToken ct)
        {
            await _quizRepo.AddAsync(quiz, ct);
        }

        public async Task AddQuestionToQuizAsync(Guid quizId, Question question, CancellationToken ct)
        {
            var quiz = await _quizRepo.GetByIdAsync(quizId, ct);
            quiz.AddQuestion(question);
            await _quizRepo.UpdateAsync(quiz, ct);
        }
    }
}