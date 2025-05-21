
namespace QuizeMC.Application.Application.Models
{
    public interface IModel<out TId> where TId : struct, IEquatable<TId>
    {
        public TId Id { get; }
    }
}