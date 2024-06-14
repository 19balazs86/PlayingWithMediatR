using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR.Pipeline;
using Serilog;
using Serilog.Events;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace PlayingWithMediatR;

public sealed class Program
{
    private const string _notFoundMessage = "The requested endpoint is not found.";

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;
        var services      = builder.Services;

        builder.Host.UseSerilog(configureLogger);

        // Add services to the container
        {
            services.AddControllers(); // Old: .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

            services.AddProblemDetails();

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddResponseCompression();

            // --> FluentValidation: Init
            // Warning: No longer recommend using auto-validation. Read the GitHub description
            // Validators cannot run asynchronous rules. If you attempt to do so, you will receive an exception at runtime.
            // https://github.com/FluentValidation/FluentValidation.AspNetCore
            // HOWEVER, in this example, the RequestValidationBehavior MediatR pipeline handles it with the ValidateAsync method.
            services
                .AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

            // --> MediatR: Add
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<Program>();

                config.Lifetime = ServiceLifetime.Scoped;

                // AddMediatR no longer scans the assemlby
                // https://github.com/jbogard/MediatR/wiki/Migration-Guide-12.0-to-12.1
                config.AddOpenBehavior(typeof(RequestPipelineBehavior<,>), ServiceLifetime.Singleton);

                config.AddOpenRequestPreProcessor(typeof(ValidationPreProcessor<>), ServiceLifetime.Singleton);
            });

            // --> EF: Use in-memory | For better performance use AddDbContextPool instead of AddDbContext
            // SQL Query in the log: "Microsoft.EntityFrameworkCore.Database.Command": "Information"
            services.AddDbContextPool<DataBaseContext>(options =>
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

        WebApplication app = builder.Build();

        // Configure the request pipeline
        {
            // First: use our custom middleware to handle exceptions.
            //app.UseExceptionHandlingMiddleware();

            // New in .NET 8
            app.UseExceptionHandler();

            // This will handle the exception, like the middleware above.
            // But in addition, throws "An unhandled exception has occurred..."
            //app.UseExceptionHandler(appBuilder => appBuilder.UseCustomErrors(env));

            app.UseRouting();

            app.UseResponseCompression();

            app.MapControllers();

            app.MapFallback(endpointNotFoundHandler);
        }

        app.Run();
    }

    private static ProblemHttpResult endpointNotFoundHandler()
    {
        return TypedResults.Problem(_notFoundMessage, statusCode: StatusCodes.Status404NotFound);
    }


    // Use the simpler method: endpointNotFoundHandler
    private static async Task pageNotFoundHandler(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode  = StatusCodes.Status404NotFound;

        var problemDetails = new ProblemDetails
        {
            Title  = "The requested endpoint is not found.",
            Status = StatusCodes.Status404NotFound,
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
    }

    private static void configureLogger(HostBuilderContext context, LoggerConfiguration configuration)
    {
        configuration
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
            //.Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}");
    }
}
