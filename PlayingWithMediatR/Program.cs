using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithMediatR.Infrastructure;
using Serilog;

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
        .UseSerilog();
  }

  public static class WebHostExtensions
  {
    public static IWebHost SeedData(this IWebHost host)
    {
      using (IServiceScope scope = host.Services.CreateScope())
      {
        IServiceProvider services = scope.ServiceProvider;
        DataBaseContext dbContext = services.GetService<DataBaseContext>();

        DataBaseInitializer.Initialize(dbContext);
      }

      return host;
    }
  }
}
