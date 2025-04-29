
namespace Domain.Exeptions
{
    public class UsernameException : DomainException
    {
        public UsernameException(string message) : base(message)
        {
        }
    }
}
