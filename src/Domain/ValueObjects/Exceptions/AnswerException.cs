
namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public class AnswerException : ValidationException
    {
        public AnswerException(string message) : base(message) { }
    }
}
