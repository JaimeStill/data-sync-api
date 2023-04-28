using Common.Schema;

namespace Process.Models;
public class Resource : Entity
{
    public int PackageId { get; set; }
    public int ResourceId { get; set; }
    public string Type { get; set; } = "Type";
    
    public Package? Package { get; set; }
}