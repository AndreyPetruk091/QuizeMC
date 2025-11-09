namespace QuizeMC.Application.Models.Common
{
    public record ApiResponse(
        bool Success,
        string Message = "",
        List<string> Errors = null!
    )
    {
        public static ApiResponse Ok(string message = "")
            => new(true, message);

        public static ApiResponse Fail(string message, List<string>? errors = null)
            => new(false, message, errors ?? new List<string>());
    }
}