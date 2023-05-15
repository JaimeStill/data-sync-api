using Sync.Hub;

namespace Common.Sync.Hub;
public abstract class ApiSyncHub<T> : SyncHub<T, IApiSyncHub<T>> { }