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

            var error = new ActionResultResponse
            {
                Code = StatusCodes.Status400BadRequest,
                Title = "Error",
                Message = exception.Message,
                
            };
            httpContext.Response.StatusCode = (int)error.Code;
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}
