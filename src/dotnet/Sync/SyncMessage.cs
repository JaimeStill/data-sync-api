namespace Sync;
public class SyncMessage<T> : ISyncMessage<T>
{
    public Guid Id { get; private set; }
    public T Data { get; set; }
    public ActionType Action { get; set; }
    public string Message { get; set; }

    public SyncMessage(
        T data,
        ActionType action,
        string? message = null
    ) {
        Id = Guid.NewGuid();
        Action = action;
        Data = data;
        Message = message ?? string.Empty;
    }
}