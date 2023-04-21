namespace Sync.Hub;
public interface ISyncHub
{
    Task Create<T>(ISyncMessage<T> message);
    Task Update<T>(ISyncMessage<T> message);
    Task Sync<T>(ISyncMessage<T> message);
    Task Delete<T>(ISyncMessage<T> message);
}