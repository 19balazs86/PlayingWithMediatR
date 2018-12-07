# Playing with MediatR

This small ASP.NET Core web application is an example for using the [MediatR](https://github.com/jbogard/MediatR "MediatR") framework.

If you are familiar with *Command Query Responsibility Segregation* (CQRS), you will find a common ground.

You can find a simple example for [FluentValidation](https://fluentvalidation.net "FluentValidation") to validate the MediatR pipeline.

In order to achive the model validation in the MediatR pipline (instead of in the APIController), you have to [disable the built-in automatic model state validation](https://www.talkingdotnet.com/disable-automatic-model-state-validation-in-asp-net-core-2-1 "disable the built-in automatic model state validation").

Article about the [Global Error Handling in ASP.NET Core Web API](https://code-maze.com/global-error-handling-aspnetcore "Global Error Handling in ASP.NET Core Web API").

Simple example for using [EntityFrameworkCore](https://docs.microsoft.com/en-us/ef/core/index "EntityFrameworkCore") in-memory database.
