namespace Sync.Hub;
public interface ISyncHub<T>
{
    Task Sync(ISyncMessage<T> message);
}