namespace QuizeMC.Infrastructure.Common
{
    public interface IProducerService<TModelEvent>
    {
        void Send(TModelEvent message);
    }
}