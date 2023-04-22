namespace Sync;
public interface ISyncMessage<T>
{
    public Guid Id { get; }
    public T Data { get; set; }
    public ActionType Action { get; set; }
    public string Message { get; set; }
}