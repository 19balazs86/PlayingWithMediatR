using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR.Pipeline;
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

        var services = builder.Services;

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
            // Show SQL Query in the log: "Microsoft.EntityFrameworkCore.Database.Command": "Information"
            services.AddDbContextPool<DataBaseContext>(options => options.UseInMemoryDatabase("dbName"));

            // --> Add: AutoMapper
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

            // This will handle the exception, like the middleware above.
            // But in addition, throws "An unhandled exception has occurred..."
            //app.UseExceptionHandler(appBuilder => appBuilder.UseCustomErrors(builder.Environment));

            app.UseExceptionHandler(); // No need to apply appBuilder.UseCustomErrors, because we use services.AddExceptionHandler<GlobalExceptionHandler>

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
}
