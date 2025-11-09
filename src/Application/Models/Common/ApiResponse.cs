namespace QuizeMC.Application.Models.Common
{
    public record ApiResponse<T>(
        bool Success,
        string Message = "",
        T? Data = default,
        List<string> Errors = null!
    )
    {
        public static ApiResponse<T> Ok(T data, string message = "")
            => new(true, message, data);

        public static ApiResponse<T> Fail(string message, List<string>? errors = null)
            => new(false, message, default, errors ?? new List<string>());
    }
}