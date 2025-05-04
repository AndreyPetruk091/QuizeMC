namespace Infrastructure
{
    public interface IProducerService<TModelEvent>
    {
        void Send(TModelEvent message);
    }
}