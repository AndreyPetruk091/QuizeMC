namespace QuizeMC.Application.Models.Admin
{
    public record AdminCreateModel(string Email, string Password, string ConfirmPassword)
        : CreateModel;
}