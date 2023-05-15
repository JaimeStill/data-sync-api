using Common.Cli;
using Contracts.App;

namespace SyncCli.Commands;
public class ProposalPingCommand : CliCommand
{
    public ProposalPingCommand() : base(
        "ping",
        "Ping the Proposal ApiSyncHub endpoint",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string sync)
    {
        await using ProposalSync client = new(sync);
        await client.Connect();
        await client.Ping();
    }
}