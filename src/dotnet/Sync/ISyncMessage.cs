namespace Sync;
public interface ISyncMessage<T>
{
    public Guid Id { get; }
    public ActionType Action { get; set; }
    public string Channel { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}