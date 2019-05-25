# Playing with MediatR

This small ASP.NET Core web application is an example for using the [MediatR](https://github.com/jbogard/MediatR "MediatR") framework.

#### What is MediatR?
> Supports request/response, commands, queries, notifications and events, synchronous and async with intelligent dispatching via C# generic variance.

If you are familiar with *Command Query Responsibility Segregation* (CQRS), you will find a common ground.

#### In the example you can find

- [FluentValidation](https://fluentvalidation.net "FluentValidation") to validate the MediatR pipeline.
- In order to achive the model validation in the MediatR pipline (instead of in the APIController), you have to [disable the built-in automatic model state validation.](https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1 "disable the built-in automatic model state validation")
- Article about the [Global Error Handling in ASP.NET Core Web API.](https://code-maze.com/global-error-handling-aspnetcore "Global Error Handling in ASP.NET Core Web API")
- [EntityFrameworkCore](https://docs.microsoft.com/en-us/ef/core/index "EntityFrameworkCore") in-memory database.
- [AutoMapper](https://github.com/AutoMapper/AutoMapper "AutoMapper") with EF.
  - Dotnet Core Tutorials blog: [Automapper in ASP.NET Core](https://dotnetcoretutorials.com/2017/09/23/using-automapper-asp-net-core "Automapper in ASP.NET Core").
  - Blog: [Simple and Fast Object Mapper](https://rehansaeed.com/a-simple-and-fast-object-mapper "Simple and Fast Object Mapper").
- How to log the generated SQL queries by the EF.
- Default 404 response. [Tips and tricks for ASP.NET Core applications](https://dusted.codes/advanced-tips-and-tricks-for-aspnet-core-applications "Tips and tricks for ASP.NET Core applications") from Dusted Codes.
- YouTube: [Clean architecture](https://www.youtube.com/watch?v=RQve_bD8X_M "Clean architecture") | GitHub repository: [NorthwindTraders](https://github.com/JasonGT/NorthwindTraders "NorthwindTraders").