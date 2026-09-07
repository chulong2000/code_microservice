using DemoApi.Domain.Exceptions;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;

namespace DemoApi.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        // Inject ILogger using Dependency Injection
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(
            exception,
            "An unhandled exception occurred while processing request {Path}.",
            httpContext.Request.Path);

            (int statusCode, string title) = exception switch
            {
                NotFoundException => ((int)HttpStatusCode.BadRequest, "Not Found Data"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized Access"),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource Not Found"),
                _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error")
            };
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message, 
                Instance = httpContext.Request.Path
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
