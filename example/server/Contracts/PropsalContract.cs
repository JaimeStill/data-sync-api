using Common.Schema;

namespace Contracts;
public class ProposalContract : Contract
{
    public string Description { get; set; } = string.Empty;
}