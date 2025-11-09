namespace QuizeMC.Application.Services.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string ErrorMessage { get; }
        public List<ValidationError> ValidationErrors { get; }

        public ServiceResult(T data)
        {
            IsSuccess = true;
            Data = data;
            ErrorMessage = string.Empty;
            ValidationErrors = new List<ValidationError>();
        }

        public ServiceResult(string errorMessage, List<ValidationError>? validationErrors = null)
        {
            IsSuccess = false;
            Data = default;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }

        public static ServiceResult<T> Success(T data) => new ServiceResult<T>(data);
        public static ServiceResult<T> Failure(string errorMessage) => new ServiceResult<T>(errorMessage);
        public static ServiceResult<T> ValidationFailure(List<ValidationError> errors)
            => new ServiceResult<T>("Validation failed", errors);
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public List<ValidationError> ValidationErrors { get; }

        public ServiceResult(bool isSuccess, string errorMessage = "", List<ValidationError>? validationErrors = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors ?? new List<ValidationError>();
        }

        public static ServiceResult Success() => new ServiceResult(true);
        public static ServiceResult Failure(string errorMessage) => new ServiceResult(false, errorMessage);
        public static ServiceResult ValidationFailure(List<ValidationError> errors)
            => new ServiceResult(false, "Validation failed", errors);
    }
}