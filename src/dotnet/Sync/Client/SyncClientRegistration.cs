using Microsoft.Extensions.DependencyInjection;

namespace Sync.Client;
public static class SyncClientRegistration
{
    public static void AddSyncClient<C>(this IServiceCollection services)
        where C : SyncClient =>
            services.AddSingleton<C>();
}