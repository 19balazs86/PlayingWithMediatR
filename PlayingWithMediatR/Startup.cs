using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR.Pipeline;
using PlayingWithMediatR.Validation;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace PlayingWithMediatR;

public sealed class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(); // Old: .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

        // --> FluentValidation: Init
        // Warning: No longer recommend using auto-validation. Read the GitHub description
        // Validators cannot run asynchronous rules. If you attempt to do so, you will receive an exception at runtime.
        // https://github.com/FluentValidation/FluentValidation.AspNetCore
        // HOWEVER, in this example, the RequestValidationBehavior MediatR pipeline handles it with the ValidateAsync method.
        services
            .AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
            .AddValidatorsFromAssemblyContaining<ProductValidator>(ServiceLifetime.Scoped);

        // --> MediatR: Add
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Startup>();

            config.AddOpenBehavior(typeof(RequestPipelineBehavior<,>), ServiceLifetime.Singleton);

            config.Lifetime = ServiceLifetime.Scoped;
        });

        // --> EF: Use in-memory database
        services.AddDbContext<DataBaseContext>(options =>
          options
            //.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted }) // EF5 + Microsoft.EntityFrameworkCore.Diagnostics
            //.UseLoggerFactory(EFSerilogLoggerProvider.LoggerFactory) // To see the EF logs.
            .UseInMemoryDatabase("dbName"));

        // --> Add AutoMapper. Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
        services.AddAutoMapper(Assembly.GetExecutingAssembly()); // AppDomain.CurrentDomain.GetAssemblies()

        // Customise: Default API behavour to let the program run the RequestValidationBehavior in the MediatR pipeline
        // Otherwise the framework will intercept the query in the ModelState filter (but using the FluentValidation)
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        // Add HostedService to seed database.
        services.AddHostedService<DatabaseSeedHostedService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // First: use our custom middleware to handle exceptions.
        app.UseExceptionHandlingMiddleware();

        // This will handle the exception, like the middleware above.
        // But in addition, throws "An unhandled exception has occurred..."
        //app.UseExceptionHandler(appBuilder => appBuilder.UseCustomErrors(env));

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            // Set-up a page not found middleware.
            // https://wakeupandcode.com/middleware-in-asp-net-core/#branches
            endpoints.MapFallback(pageNotFoundHandler);
        });
    }

    private static async Task pageNotFoundHandler(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        var problemDetails = new ProblemDetails
        {
            Title = "The requested endpoint is not found.",
            Status = StatusCodes.Status404NotFound,
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
    }
}
