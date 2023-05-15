using Sync.Hub;

namespace Common.Sync.Hub;
public abstract class ProcessSyncHub<T> : SyncHub<T, IProcessSyncHub<T>> { }