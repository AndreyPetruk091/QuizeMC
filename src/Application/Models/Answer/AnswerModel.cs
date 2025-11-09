namespace QuizeMC.Application.Models.Answer
{
    public record AnswerModel(Guid Id, string Text, Guid QuestionId) : Model(Id);
}