namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public static class ExceptionMessages
    {
        // Общие
        public const string NullValue = "{0} cannot be null";
        public const string EmptyValue = "{0} cannot be empty";

        // Username
        public const string UsernameTooShort = "Username must be at least {0} characters";
        public const string UsernameTooLong = "Username cannot exceed {0} characters";
        public const string UsernameInvalidChars = "Username contains invalid characters";

        public static string Format(string template, params object[] args)
            => string.Format(template, args);
    }
}