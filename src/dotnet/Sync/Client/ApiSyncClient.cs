using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sync.Client;
public abstract class ApiSyncClient<T> : SyncClient<T>, IApiSyncClient<T>
{
    public SyncAction OnAdd { get; protected set; }
    public SyncAction OnUpdate { get; protected set; }
    public SyncAction OnRemove { get; protected set; }

    public ApiSyncClient(string endpoint) : base(endpoint)
    {
        InitializeActions();
    }

    public async Task Add(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendAdd", message);

    public async Task Update(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendUpdate", message);

    public async Task Remove(ISyncMessage<T> message) =>
        await connection.InvokeAsync("SendRemove", message);

    [MemberNotNull(
        nameof(OnAdd),
        nameof(OnUpdate),
        nameof(OnRemove)
    )]
    protected override void InitializeActions()
    {
        base.InitializeActions();
        OnAdd = new("Add", connection);
        OnUpdate = new("Update", connection);
        OnRemove = new("Remove", connection);
    }

    protected override void DisposeEvents()
    {
        base.DisposeEvents();
        OnAdd.Dispose();
        OnUpdate.Dispose();
        OnRemove.Dispose();
    }
}