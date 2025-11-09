namespace QuizeMC.Application.Models.Category
{
    public record CategoryModel(
        Guid Id,
        string Name,
        string Description,
        DateTime CreatedAt,
        Guid CreatedByAdminId,
        string CreatedByAdminEmail,
        int QuizzesCount
    ) : Model(Id);
}