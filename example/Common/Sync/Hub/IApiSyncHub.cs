using Sync;
using Sync.Hub;

namespace Common.Sync.Hub;
public interface IApiSyncHub<T> : ISyncHub<T>
{
    Task Add(ISyncMessage<T> message);
    Task Update(ISyncMessage<T> message);
    Task Remove(ISyncMessage<T> message);
}