
namespace QuizeMC.Domain.Exceptions
{
    public class UsernameException : DomainException
    {
        public UsernameException(string message) : base(message) { }
    }
}