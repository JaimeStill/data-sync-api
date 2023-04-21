namespace Sync.Hub;
public interface ISyncHub<T>
{
    Task Add(ISyncMessage<T> message);
    Task Update(ISyncMessage<T> message);
    Task Sync(ISyncMessage<T> message);
    Task Remove(ISyncMessage<T> message);
}