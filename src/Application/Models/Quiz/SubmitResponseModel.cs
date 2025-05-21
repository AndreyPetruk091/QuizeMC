namespace QuizeMC.Application.Models.Quiz
{
    public record SubmitResponseModel(
        Guid QuizId,
        Guid QuestionId,
        Guid ParticipantId,
        int SelectedAnswerIndex
    );
}