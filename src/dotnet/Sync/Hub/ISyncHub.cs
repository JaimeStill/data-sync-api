namespace Sync.Hub;
public interface ISyncHub
{
    Task Add<T>(ISyncMessage<T> message);
    Task Update<T>(ISyncMessage<T> message);
    Task Sync<T>(ISyncMessage<T> message);
    Task Remove<T>(ISyncMessage<T> message);
}