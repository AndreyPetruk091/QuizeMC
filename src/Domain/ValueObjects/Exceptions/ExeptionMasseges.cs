namespace QuizeMC.Domain.ValueObjects.Exceptions
{
    public static class ExceptionMessages
    {
        // Общие
        public const string NullValue = "{0} cannot be null";
        public const string EmptyValue = "{0} cannot be empty";
        public const string TooLong = "{0} cannot exceed {1} characters";
        public const string InvalidFormat = "Invalid {0} format";

        // Email
        public const string EmailInvalid = "Invalid email format";

        // Password
        public const string PasswordHashInvalid = "Invalid password hash format";

        public static string Format(string template, params object[] args)
            => string.Format(template, args);
    }
}