namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public class AnswerIndexException : ValidationException
    {
        public AnswerIndexException(string message) : base(message) { }
    }
}
