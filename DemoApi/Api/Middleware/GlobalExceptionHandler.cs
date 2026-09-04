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

            (int statusCode, string message) = exception switch
            {
                NotFoundException => ((int)HttpStatusCode.BadRequest, exception.Message),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Unauthorized Access"),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Resource Not Found"),
                _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error")
            };


            var error = new ActionResultResponse
            {
                Code = statusCode,
                Title = "Error",
                Message = message,
                
            };
            httpContext.Response.StatusCode = (int)error.Code;
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}
