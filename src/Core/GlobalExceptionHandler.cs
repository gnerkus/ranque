using Entities;
using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace streak
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
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
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            _logger.LogError("Something went wrong: {@Exception}", exceptionHandlerFeature.Error);

            await httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exceptionHandlerFeature.Error.Message
            }.ToString(), cancellationToken);

            return true;
        }
    }
}