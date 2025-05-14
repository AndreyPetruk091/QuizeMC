using System.Text.RegularExpressions;
using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects.Validators
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