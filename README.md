# Playing with MediatR

This small ASP.NET web application is an example for using the [MediatR](https://github.com/jbogard/MediatR) framework.

[Separate branch](https://github.com/19balazs86/PlayingWithMediatR/tree/netcoreapp2.2) with the .NET Core 2.2 version.

#### What is MediatR?
> Supports request/response, commands, queries, notifications and events, sync and async with intelligent dispatching via C# generic variance.

If you are familiar with *Command Query Responsibility Segregation* (CQRS), you will find a common ground.

#### In the example you can find

- [FluentValidation](https://fluentvalidation.net) to validate the MediatR pipeline
- [Pipeline validation with FluentValidation](https://code-maze.com/cqrs-mediatr-fluentvalidation) 📓*CodeMaze*
- In order to achieve the model validation in the MediatR pipeline (instead of in the APIController), you have to [disable the built-in automatic model state validation](https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1)
- Custom error handling
  - [Creating a custom ErrorHandlerMiddleware function](https://andrewlock.net/creating-a-custom-error-handler-middleware-function) 📓*AndrewLock*
  - [Global Error Handling in ASP.NET Core Web API](https://code-maze.com/global-error-handling-aspnetcore) 📓*CodeMaze*
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/index) 📚
  - 📓*CodeMaze* - [Code-First approach](https://code-maze.com/net-core-web-api-ef-core-code-first/) and [Entity Framework Core Series](https://code-maze.com/entity-framework-core-series/)
  - [Learn Entity Framework Core](https://www.learnentityframeworkcore.com/) 📓
- [AutoMapper](https://github.com/AutoMapper/AutoMapper) with EF
  - [Getting Started with AutoMapper in ASP.NET Core](https://code-maze.com/automapper-net-core/) 📓*CodeMaze*
  - [Automapper in ASP.NET Core](https://dotnetcoretutorials.com/2017/09/23/using-automapper-asp-net-core) 📓*.NetCoreTutorials*
  - [Simple and Fast Object Mapper](https://rehansaeed.com/a-simple-and-fast-object-mapper) 📓*RehanSaeed*
- [Running async tasks on app Startup](https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3) 📓*AndrewLock*
- How to log the generated SQL queries by the EF
- Default 404 response. [Tips and tricks for ASP.NET Core applications](https://dusted.codes/advanced-tips-and-tricks-for-aspnet-core-applications) 📓*DustedCodes*
- [Clean architecture](https://www.youtube.com/watch?v=5OtUm1BLmG0) 📽️1h | [NorthwindTraders](https://github.com/JasonGT/NorthwindTraders) 👤*JasonTaylor*
- [Generating realistic data with Bogus](https://code-maze.com/data-generation-bogus-dotnet) 📓*CodeMaze*
---
- [Publish MediatR Notifications in parallel](https://www.milanjovanovic.tech/blog/how-to-publish-mediatr-notifications-in-parallel) 📓*Milan*
