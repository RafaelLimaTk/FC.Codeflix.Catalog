﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FC.Codeflix.Catalog.Api.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;
    public ApiGlobalExceptionFilter(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();
        var exception = context.Exception;

        if (_hostEnvironment.IsDevelopment())
            details.Extensions.Add("StackTrace", exception.StackTrace);

        if (exception is EntityValidationException)
        {
            details.Title = "One or more validation errors occurred.";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = exception!.Message;
        }
        else if (exception is NotFoundException)
        {
            details.Title = "Not Found";
            details.Status = StatusCodes.Status404NotFound;
            details.Type = "NotFound";
            details.Detail = exception!.Message;
        }
        else if (exception is RelatedAggregateException)
        {
            details.Title = "Invalid Related Aggregate";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "RelatedAggregate";
            details.Detail = exception!.Message;
        }
        else
        {
            details.Title = "An unexpected error ocurred";
            details.Status = StatusCodes.Status500InternalServerError;
            details.Type = "UnexpectedError";
            details.Detail = exception.Message;
        }

        context.HttpContext.Response.StatusCode = (int)details.Status!;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}
