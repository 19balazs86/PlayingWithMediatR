using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.Infrastructure;
using PlayingWithMediatR.MediatR.Pipeline;
using PlayingWithMediatR.Validation;
using System.Reflection;

namespace PlayingWithMediatR
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); // Old: .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

            // --> FluentValidation: Init
            // Warning: No longer recommend using auto-validation. Read the GitHub description
            // https://github.com/FluentValidation/FluentValidation.AspNetCore
            // Asynchronous rules in validators will not be able to run. You will receive an exception at runtime
            // BUT in this example the RequestValidationBehavior MediatR pipeline handle is with ValidateAsync method
            services
                .AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssemblyContaining<ProductValidator>();

            // --> MediatR: Add pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            // --> MediatR: Add
            services.AddMediatR(Assembly.GetExecutingAssembly());

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
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("The requested endpoint is not found.");
        }
    }
}
