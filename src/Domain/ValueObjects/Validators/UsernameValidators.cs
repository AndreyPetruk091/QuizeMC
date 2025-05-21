using System.Text.RegularExpressions;
using QuizeMC.Domain.ValueObjects.Base;
using QuizeMC.Domain.ValueObjects.Exceptions;

namespace QuizeMC.Domain.ValueObjects.Validators
{
    public class UsernameValidator : IValidator<string>
    {
        private const int MinLength = 3;
        private const int MaxLength = 50;
        private static readonly Regex ValidCharsRegex = new(@"^[a-zA-Z0-9_\-\.]+$");

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new UsernameException(ExceptionMessages.EmptyValue);

            if (value.Length < MinLength)
                throw new UsernameException(
                    ExceptionMessages.Format(ExceptionMessages.UsernameTooShort, MinLength));

            if (value.Length > MaxLength)
                throw new UsernameException(
                    ExceptionMessages.Format(ExceptionMessages.UsernameTooLong, MaxLength));

            if (!ValidCharsRegex.IsMatch(value))
                throw new UsernameException(ExceptionMessages.UsernameInvalidChars);
        }
    }
}