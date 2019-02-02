using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithMediatR.Infrastructure;
using Serilog;
using Serilog.Events;

namespace PlayingWithMediatR
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().SeedData().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseSerilog(configureLogger);

    private static void configureLogger(WebHostBuilderContext context, LoggerConfiguration configuration)
    {
      configuration
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        //.Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}");
    }
  }

  public static class WebHostExtensions
  {
    public static IWebHost SeedData(this IWebHost host)
    {
      using (IServiceScope scope = host.Services.CreateScope())
      {
        IServiceProvider services = scope.ServiceProvider;
        DataBaseContext dbContext = services.GetRequiredService<DataBaseContext>();

        // Difference between GetService() and GetRequiredService()
        // https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core

        dbContext.Initialize();
      }

      return host;
    }
  }
}
