namespace Common.Schema;
public abstract class Entity : IContract
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string DateCreated { get; set; } = string.Empty;
    public string LastModified { get; set; } = string.Empty;

    public virtual void OnSaving()
    {
        Type = GetType().FullName ?? "Entity";
        LastModified = JsDateEncoder.JsDate(DateTime.Now);

        if (string.IsNullOrWhiteSpace(DateCreated) || Id < 1)
            DateCreated = LastModified;
    }
}