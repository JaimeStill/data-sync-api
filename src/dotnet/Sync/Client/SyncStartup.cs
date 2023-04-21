using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sync.Client;
public abstract class SyncStartup<T> : BackgroundService
    where T : SyncClient
{
    private readonly IServiceProvider provider;
    private readonly IHostApplicationLifetime lifetime;
    private readonly TaskCompletionSource source = new();

    public SyncStartup(IServiceProvider provider, IHostApplicationLifetime lifetime)
    {
        this.provider = provider;
        this.lifetime = lifetime;
        
        this.lifetime.ApplicationStarted.Register(source.SetResult);
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        try
        {
            await source.Task.ConfigureAwait(false);
            using IServiceScope scope = provider.CreateScope();

            T client = scope.ServiceProvider.GetRequiredService<T>();

            if (client is not null)
                await client.Connect(token);
        }
        catch when (token.IsCancellationRequested)
        {
            return;
        }
    }
}