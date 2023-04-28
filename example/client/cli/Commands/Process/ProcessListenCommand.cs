using Common.Cli;
using Contracts.Process;

namespace SyncCli.Commands;
public class ProcessListenCommand : CliCommand
{
    public ProcessListenCommand() : base(
        "listen",
        "Listen to a Process SyncHub endpoint",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string sync)
    {
        await using ProcessSync client = new(sync);
        await client.Connect();
        Console.WriteLine("Press Ctrl + C to exit...");

        while (true) { }
    }
}