namespace QuizeMC.Domain.Exceptions
{
    public class CategoryException : DomainException
    {
        public CategoryException(string message, Guid? categoryId = null)
            : base(message, categoryId) { }
    }
}