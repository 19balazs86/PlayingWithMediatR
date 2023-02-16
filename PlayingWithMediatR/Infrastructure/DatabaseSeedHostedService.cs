namespace PlayingWithMediatR.Infrastructure;

public sealed class DatabaseSeedHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DatabaseSeedHostedService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        // Difference between GetService() and GetRequiredService()
        // https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core
        DataBaseContext dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

        await dbContext.SeedAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
