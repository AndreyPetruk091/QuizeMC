

namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public class QuizTitleException : ValidationException
    {
        public QuizTitleException(string message) : base(message) { }
    }
}
