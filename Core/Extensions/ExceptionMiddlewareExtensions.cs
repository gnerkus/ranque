using System.Net;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Diagnostics;

namespace streak.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager
            logger)
        {
            app.UseExceptionHandler(
                appBuilder =>
                {
                    appBuilder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";

                            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (contextFeature != null)
                            {
                                logger.LogError($"Something went wrong: {contextFeature.Error}");

                                await context.Response.WriteAsync(new ErrorDetails
                                {
                                    StatusCode = context.Response.StatusCode,
                                    Message = "Internal Server Error."
                                }.ToString());
                            }
                        });
                }
            );
        }
    }
}