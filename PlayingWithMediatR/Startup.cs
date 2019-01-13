using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR;
using PlayingWithMediatR.MediatR.Pipeline;
using PlayingWithMediatR.Validation;
using Serilog;
using Serilog.Events;

namespace PlayingWithMediatR
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      // --> Init: Logger
      initLogger();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      // --> FluentValidation: Init (nuget: FluentValidation.AspNetCore)
      services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

      // --> MediatR: Add pipeline behaviors
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

      // --> MediatR: Add
      services.AddMediatR(typeof(GetAllProduct).GetTypeInfo().Assembly);

      // --> EF: Use in-memory database
      services.AddDbContext<DataBaseContext>(options =>
        options
          //.UseLoggerFactory(EFSerilogLoggerProvider.LoggerFactory) // To see the EF logs.
          .UseInMemoryDatabase("dbName"));

      // --> Add AutoMapper. Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
      services.AddAutoMapper();

      // Customise: Default API behavour to let the program run the RequestValidationBehavior in the MediatR pipeline
      // Otherwise the framework will intercept the query in the ModelState filter (but using the FluentValidation)
      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.SuppressModelStateInvalidFilter = true;
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      // First: use our custom middleware to handle exceptions.
      app.UseExceptionHandlingMiddleware();

      // This will handle the exception, like the middleware above. 
      // But in addition, throws "An unhandled exception has occurred..."
      //app.UseExceptionHandler(appBuilder => appBuilder.Run(ExceptionHandlingMiddleware.ApplicationBuilderRun));

      app.UseMvc();
    }

    private void initLogger()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}")
        .CreateLogger();
    }
  }
}
