using System.Text.RegularExpressions;
using ValueObjects.Base;
using ValueObjects.Exceptions;

namespace ValueObjects.Validators
{
    public class UsernameValidator : IValidator <string>
    {
        private const int MinLength = 3;
        private const int MaxLength = 50;
        private const string AllowedCharsPattern = @"^[a-zA-Z0-9_\-\.]+$";

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new UsernameException("Username cannot be empty");

            if (value.Length < MinLength)
                throw new UsernameException($"Username must be at least {MinLength} characters");

            if (value.Length > MaxLength)
                throw new UsernameException($"Username cannot exceed {MaxLength} characters");

            if (!Regex.IsMatch(value, AllowedCharsPattern))
                throw new UsernameException("Username contains invalid characters");
        }
    }
}
