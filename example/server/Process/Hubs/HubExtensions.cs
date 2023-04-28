namespace Process.Hubs;
public static class HubExtensions
{
    public static void MapHubs(this WebApplication app)
    {
        app.MapHub<ProcessHub>("/sync/process");
    }
}