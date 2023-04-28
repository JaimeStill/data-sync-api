using Common.Cli;
using Contracts.Sync;

namespace SyncCli.Commands;
public class ProposalListenCommand : CliCommand
{
    public ProposalListenCommand() : base(
        "listen",
        "Listen to a Proposal ApiSyncHub endpoint",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string sync)
    {
        await using ProposalSync client = new(sync);
        await client.Connect();        
        Console.WriteLine("Press Ctrl + C to exit...");

        while (true) { }
    }
}