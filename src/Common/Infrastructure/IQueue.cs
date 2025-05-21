

namespace Infrastructure
{
    public interface IQueue<T>
    {
        public T QueueName { get; }
    }
}