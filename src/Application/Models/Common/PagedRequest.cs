namespace QuizeMC.Application.Models.Common
{
    public record PagedRequest(
        int PageNumber = 1,
        int PageSize = 20,
        string? SearchTerm = null,
        string? SortBy = null,
        bool SortDescending = false
    );
}