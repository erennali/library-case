using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var details = new ValidationProblemDetails(ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Validation failed"
            };
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (KeyNotFoundException ex)
        {
            var details = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Not found",
                Detail = ex.Message
            };
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            var details = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An unexpected error occurred"
            };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(details);
        }
    }
}


