using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR.Pipeline;
using System.Diagnostics;
using System.Reflection;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace PlayingWithMediatR;

public sealed class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IServiceCollection services   = builder.Services;

        // Add services to the container
        {
            services.AddControllers(); // Old: .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

            services.AddProblemDetails(configureProblemDetails);

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

            // This will handle the exception, similar to the middleware above.
            // However, it also throws the message 'An unhandled exception has occurred'
            //app.UseExceptionHandler(appBuilder => appBuilder.UseCustomErrors(builder.Environment));

            app.UseExceptionHandler(); // There is no need to apply appBuilder.UseCustomErrors, because we are using services.AddExceptionHandler<GlobalExceptionHandler>

            app.UseStatusCodePages(); // Returns the Problem Details response for unsuccessful empty responses

            app.UseResponseCompression();

            app.MapControllers();

            app.MapFallback(endpointNotFoundHandler);
        }

        app.Run();
    }

    private static void configureProblemDetails(ProblemDetailsOptions options)
    {
        options.CustomizeProblemDetails = context =>
        {
            HttpContext httpContext = context.HttpContext;

            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            context.ProblemDetails.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";

            context.ProblemDetails.Extensions.TryAdd("requestId", httpContext.TraceIdentifier);

            context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
        };
    }

    private static ProblemHttpResult endpointNotFoundHandler()
    {
        return TypedResults.Problem(_notFoundMessage, statusCode: Status404NotFound);
    }


    // Use the simpler method: endpointNotFoundHandler
    //private static async Task pageNotFoundHandler(HttpContext context)
    //{
    //    context.Response.ContentType = MediaTypeNames.Application.Json;
    //    context.Response.StatusCode  = Status404NotFound;

    //    var problemDetails = new ProblemDetails
    //    {
    //        Title  = _notFoundMessage,
    //        Status = Status404NotFound,
    //    };

    //    await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails);
    //}

    private const string _notFoundMessage = "The requested endpoint is not found.";
}