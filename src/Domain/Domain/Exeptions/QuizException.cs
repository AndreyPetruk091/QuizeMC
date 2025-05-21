
namespace QuizeMC.Domain.Exceptions
{
    public class QuizException : DomainException
    {
        public QuizException(string message, Guid? quizId = null)
            : base(message, quizId) { }
    }
}