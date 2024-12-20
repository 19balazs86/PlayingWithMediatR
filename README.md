# Playing with MediatR

This ASP.NET web application demonstrates how to use the [MediatR](https://github.com/jbogard/MediatR) framework

## What is MediatR?
> Supports request/response, commands, queries, notifications and events, sync and async with intelligent dispatching via C# generic variance.

If you are familiar with *Command Query Responsibility Segregation* (CQRS), you will find common ground.

## In the example, you will find

- [FluentValidation](https://fluentvalidation.net) to validate the MediatR pipeline
- [Pipeline validation with FluentValidation](https://code-maze.com/cqrs-mediatr-fluentvalidation) 📓*CodeMaze*
- To achieve model validation in the MediatR pipeline (instead of in the APIController), you need to [disable the built-in automatic model state validation](https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1)
- Error handling
  - [Problem Details and IExceptionHandler](https://www.milanjovanovic.tech/blog/problem-details-for-aspnetcore-apis) 📓*Milan's newsletter*
  - [New way in ASP.NET 8 using IExceptionHandler](https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8) 📓*Milan's newsletter*
  - [Translating exceptions into Problem Details responses](https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses) 📓*Tim Deschryver*
  - [Using IRequestExceptionHandler in MediatR requests](https://code-maze.com/csharp-global-exception-handling-for-mediatr-requests/) 📓*CodeMaze*
  - [Creating a custom ErrorHandlerMiddleware function](https://andrewlock.net/creating-a-custom-error-handler-middleware-function) 📓*AndrewLock*
  - [Global Error Handling in ASP.NET Core Web API](https://code-maze.com/global-error-handling-aspnetcore) 📓*CodeMaze*
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/index) 📚
  - 📓*CodeMaze* - [Code-First approach](https://code-maze.com/net-core-web-api-ef-core-code-first/) and [Entity Framework Core Series](https://code-maze.com/entity-framework-core-series/)
  - [Learn Entity Framework Core](https://www.learnentityframeworkcore.com/) 📓
- [AutoMapper](https://github.com/AutoMapper/AutoMapper) with EF
  - [Getting Started with AutoMapper in ASP.NET Core](https://code-maze.com/automapper-net-core/) 📓*CodeMaze*
  - [Performance comparison of different mappers](https://youtu.be/U8gSdQN2jWI) 📽️*12m - Nick Chapsas - Mapperly, Automapper, AgileMapper, TinyMapper, Mapster, MappingGenerator*
- [Response compression](https://www.milanjovanovic.tech/blog/response-compression-in-aspnetcore) 📓*Milan's newsletter*
- [Running async tasks on app Startup](https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3) 📓*AndrewLock - Using HostedService*
- How to log the generated SQL queries by the EF
- Default 404 response. [Tips and tricks for ASP.NET Core applications](https://dusted.codes/advanced-tips-and-tricks-for-aspnet-core-applications) 📓*DustedCodes*
- [Generating realistic data with Bogus](https://code-maze.com/data-generation-bogus-dotnet) 📓*CodeMaze*

---

- [Publish MediatR Notifications in parallel](https://www.milanjovanovic.tech/blog/how-to-publish-mediatr-notifications-in-parallel) 📓*Milan's newsletter*
- Clean architecture - JasonTaylor
  - [Unleashing Clean Architecture in .NET 8](https://youtu.be/yB01HaG0i0w) 📽️*45m*
  - [CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture) 👤*jasontaylordev*
