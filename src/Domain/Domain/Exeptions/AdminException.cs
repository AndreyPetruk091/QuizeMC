namespace QuizeMC.Domain.Exceptions
{
    public class AdminException : DomainException
    {
        public AdminException(string message, Guid? adminId = null)
            : base(message, adminId) { }
    }
}