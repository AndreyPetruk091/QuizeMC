namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public class QuestionTextException: ValidationException
    {
        public QuestionTextException(string message) : base(message) { }
    }
}
