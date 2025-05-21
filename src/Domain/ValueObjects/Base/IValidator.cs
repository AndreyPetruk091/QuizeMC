
namespace QuizeMC.Domain.ValueObjects.Base
{

    public interface IValidator<T>
    {
        void Validate(T value);
    }
}
