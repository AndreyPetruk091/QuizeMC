namespace QuizeMC.Application.Models.Quiz
{
    public record QuizUpdateModel(string? Title, string? Description, Guid? CategoryId);
}