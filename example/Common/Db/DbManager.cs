using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Common.Db;
public class DbManager<T> : IAsyncDisposable
    where T : DbContext
{
    readonly bool destroy;
    public T? Context { get; private set; }
    public string? Connection => Context?.Database.GetConnectionString();

    static string? GetConnectionString(string connectionName, bool isUnique)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("connections.json")
            .AddEnvironmentVariables()
            .Build();

        string? connection = config.GetConnectionString(connectionName);

        if (isUnique)
            connection = $"{connection}-{Guid.NewGuid()}";

        return connection;
    }

    static T? GetDbContext(string? connection)
    {
        if (string.IsNullOrWhiteSpace(connection))
            return null;

        DbContextOptionsBuilder<T> builder = new DbContextOptionsBuilder<T>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSqlServer(connection);

        return (T?)Activator.CreateInstance(typeof(T), builder.Options);
    }

    public DbManager(
        string connectionName = "App",
        bool destroy = false,
        bool isUnique = false
    )
    {
        this.destroy = destroy;

        Context = GetDbContext(
            GetConnectionString(connectionName, isUnique)
        );
    }

    public Task<bool> Destroy() => Context!.Database.EnsureDeletedAsync();

    public async Task<bool> Initialize()
    {
        try
        {
            if (Context is null)
                return false;

            if (destroy)
                await Destroy();

            await Context.Database.MigrateAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (destroy)
            await Destroy();

        Context?.Dispose();

        GC.SuppressFinalize(this);
    }

    public static async Task Execute(string[] args)
    {
        try
        {
            string connectionName = args.Length > 0
                ? args[0]
                : "App";

            bool destroy = args.Length > 1
                && bool.Parse(args[1]);

            bool unique = args.Length > 2
                && bool.Parse(args[2]);

            await using DbManager<T> manager = new(
                connectionName,
                destroy,
                unique
            );

            if (manager.Context is not null)
            {
                Console.WriteLine("Initializing database and applying migrations");

                await manager.Initialize();
                await manager.Context.Seed();
            }
            else Console.WriteLine("Manager was unable to initialize a DbContext");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while seeding the database", ex);
        }
    }
}