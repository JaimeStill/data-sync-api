namespace Sync;
public class SyncMessage<T> : ISyncMessage<T>
{
    public Guid Id { get; private set; }
    public ActionType Action { get; set; }
    public string Channel { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public SyncMessage(
        string channel,
        T data,
        ActionType action,
        string? message = null
    ) {
        Id = Guid.NewGuid();
        Channel = channel;
        Action = action;
        Data = data;
        Message = message ?? string.Empty;
    }
}