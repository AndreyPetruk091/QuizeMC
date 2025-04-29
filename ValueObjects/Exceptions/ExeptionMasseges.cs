

namespace ValueObjects.Exceptions
{
    public static class ExeptionMasseges
    {
        // Общие сообщения
        public const string NullValue = "{0} не может быть null";
        public const string EmptyValue = "{0} не может быть пустым";

        // Сообщения для Username
        public const string UsernameTooShort = "Username должен содержать минимум {0} символов";
        public const string UsernameTooLong = "Username должен содержать максимум {0} символов";
        public const string UsernameInvalidChars = "Username может содержать только буквы, цифры, '-', '_' и '.'";

        public const string EmptyAnswer = "Answer cannot be empty";
        public const string InvalidAnswerIndex = "Answer index must be between 0 and {0}";
        public const string EmptyQuestionText = "Question text cannot be empty";
        public const string QuestionTextTooLong = "Question text exceeds maximum length of {0} characters";
        public const string EmptyQuizTitle = "Quiz title cannot be empty";
        public const string QuizTitleTooLong = "Quiz title exceeds maximum length of {0} characters";

        public static string Format(string template, params object[] args)
            => string.Format(template, args);
    }
}
