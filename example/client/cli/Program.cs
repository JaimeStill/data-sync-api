using Common.Cli;
using SyncCli.Commands;

await new CliApp(
    "Data Sync CLI",
    new()
    {
        new ProposalCommand()
    }
).InvokeAsync(args);
