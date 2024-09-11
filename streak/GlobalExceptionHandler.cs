using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace streak
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerManager _logger;

        public GlobalExceptionHandler(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";

            var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            if (exceptionHandlerFeature == null) return true;
            httpContext.Response.StatusCode = exceptionHandlerFeature.Error switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            
            _logger.LogError($"Something went wrong: {exceptionHandlerFeature.Error}");

            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exceptionHandlerFeature.Error.Message
            }.ToString(), cancellationToken);

            return true;
        }
    }
}