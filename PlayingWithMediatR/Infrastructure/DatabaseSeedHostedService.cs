using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlayingWithMediatR.Infrastructure
{
  public class DatabaseSeedHostedService : IHostedService
  {
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSeedHostedService(IServiceProvider serviceProvider)
      => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      using IServiceScope scope = _serviceProvider.CreateScope();

      // Difference between GetService() and GetRequiredService()
      // https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core
      DataBaseContext dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

      await dbContext.SeedAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
      => Task.CompletedTask;
  }
}
