namespace QuizeMC.Application.Models.Admin
{
    public record AdminUpdateModel(
        string? Email,
        string? CurrentPassword,
        string? NewPassword,
        string? ConfirmNewPassword
    );
}