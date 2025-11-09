namespace QuizeMC.Application.Services.Common
{
    public class ValidationError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }
        public string ErrorCode { get; }

        public ValidationError(string propertyName, string errorMessage, string errorCode = "")
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }
    }
}