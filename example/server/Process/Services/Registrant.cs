using Common.Services;

namespace Process.Services;
public class Registrant : ServiceRegistrant
{
    public Registrant(IServiceCollection services) : base(services) { }

    public override void Register()
    {
        services.AddScoped<ProcessSyncService>();
    }
}