using System.Text.RegularExpressions;
using GHM.Infrastructure.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;

namespace DemoApi.Api.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var error = new ActionResultResponse(-500, "Có lỗi xảy ra");
            await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);
            return true;
        }
    }
}
