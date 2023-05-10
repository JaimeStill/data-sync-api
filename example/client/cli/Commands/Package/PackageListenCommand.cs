using Common.Cli;
using Contracts.Process;

namespace SyncCli.Commands;
public class PackageListenCommand : CliCommand
{
    public PackageListenCommand() : base(
        "listen",
        "Listen to a Package SyncHub endpoint",
        new Func<string, Task>(Call)
    ) { }

    static async Task Call(string sync)
    {
        await using PackageSync client = new(sync);
        await client.Connect();
        Console.WriteLine("Press Ctrl + C to exit...");

        while (true) { }
    }
}