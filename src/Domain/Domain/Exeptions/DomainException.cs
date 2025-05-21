namespace QuizeMC.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public Guid? EntityId { get; }

        public DomainException(string message, Guid? entityId = null)
            : base(message)
        {
            EntityId = entityId;
        }

        public DomainException(string message, Exception inner, Guid? entityId = null)
            : base(message, inner)
        {
            EntityId = entityId;
        }
    }
}