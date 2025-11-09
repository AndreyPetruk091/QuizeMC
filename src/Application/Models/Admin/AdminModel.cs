namespace QuizeMC.Application.Models.Admin
{
    public record AdminModel(
        Guid Id,
        string Email,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? LastLoginAt,
        int CreatedQuizzesCount,
        int CreatedCategoriesCount
    ) : Model(Id);
}