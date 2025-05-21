namespace QuizeMC.Domain.Exceptions
{
    public class QuestionException : DomainException
    {
        public QuestionException(string message) : base(message) { }
    }
}