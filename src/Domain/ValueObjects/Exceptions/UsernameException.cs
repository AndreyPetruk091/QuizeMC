
namespace QuizeMC.Domain.ValueObjects.Exceptions

{
    public class UsernameException : ValidationException
    {
        public UsernameException(string message) : base(message) { }
        public UsernameException(string message, Exception inner) : base(message, inner) { }
    }
}
