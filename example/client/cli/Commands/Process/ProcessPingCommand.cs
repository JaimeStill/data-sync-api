using Common.Cli;
using Contracts.Process;

namespace SyncCli.Commands;
public class ProcessPingCommand : CliCommand
{
    public ProcessPingCommand() : base(
        "ping",
        "Ping the Process SyncHub endpoint",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string sync)
    {
        await using PackageSync client = new(sync);
        await client.Connect();
        await client.Ping();
    }
}