using Common.Schema;

namespace App.Models;
public class Proposal : Entity
{
    public string Description { get; set; } = string.Empty;
}