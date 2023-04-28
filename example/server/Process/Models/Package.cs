using Common.Schema;
using Contracts.Process;

namespace Process.Models;
public class Package : Entity
{
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ProcessState State { get; set; } = ProcessState.Pending;

    public ICollection<Resource>? Resources { get; set; }
}