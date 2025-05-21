
namespace QuizeMC.Application.Application.Models
{
    public interface IQuestionModel<out TId> where TId : struct, IEquatable<TId>
    {
        TId Id { get; }
        string Text { get; }
    }
}
