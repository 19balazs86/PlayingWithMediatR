# Playing with MediatR

This small ASP.NET Core web application is an example for using the [MediatR](https://github.com/jbogard/MediatR) framework.

#### What is MediatR?
> Supports request/response, commands, queries, notifications and events, sync and async with intelligent dispatching via C# generic variance.

If you are familiar with *Command Query Responsibility Segregation* (CQRS), you will find a common ground.

#### In the example you can find

- [FluentValidation](https://fluentvalidation.net) to validate the MediatR pipeline.
- In order to achieve the model validation in the MediatR pipeline (instead of in the APIController), you have to [disable the built-in automatic model state validation.](https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1)
- Article about the [Global Error Handling in ASP.NET Core Web API.](https://code-maze.com/global-error-handling-aspnetcore)
- Entity Framework:
  - [MS Docs](https://docs.microsoft.com/en-us/ef/core/index).
  - [Learn Entity Framework Core](https://www.learnentityframeworkcore.com/).
  - Code Maze: [Code-First approach](https://code-maze.com/net-core-web-api-ef-core-code-first/) | [Entity Framework Core Series](https://code-maze.com/entity-framework-core-series/).
- [AutoMapper](https://github.com/AutoMapper/AutoMapper) with EF:
  - .Net Core Tutorials blog: [Automapper in ASP.NET Core](https://dotnetcoretutorials.com/2017/09/23/using-automapper-asp-net-core).
  - Blog: [Simple and Fast Object Mapper](https://rehansaeed.com/a-simple-and-fast-object-mapper).
- How to log the generated SQL queries by the EF.
- Dusted Codes: Default 404 response. [Tips and tricks for ASP.NET Core applications](https://dusted.codes/advanced-tips-and-tricks-for-aspnet-core-applications).
- YouTube: [Clean architecture](https://www.youtube.com/watch?v=RQve_bD8X_M) | GitHub: [NorthwindTraders](https://github.com/JasonGT/NorthwindTraders).
