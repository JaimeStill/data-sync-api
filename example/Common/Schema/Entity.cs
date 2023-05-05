namespace Common.Schema;
public abstract class Entity : IContract
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type => GetType().FullName ?? "Entity";
    public string DateCreated { get; set; } = string.Empty;
    public string LastModified { get; set; } = string.Empty;

    public virtual void OnSaving()
    {
        LastModified = JsDateEncoder.JsDate(DateTime.Now);

        if (string.IsNullOrWhiteSpace(DateCreated) || Id < 1)
            DateCreated = LastModified;
    }
}