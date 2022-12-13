using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tazeez.Common.Extensions;
using Serilog;
using HealthCare_Core.Managers.Interfaces;
using HealthCare_ModelView;
using HealthCare_Common.Extensions;
using HealthCare_Common.Logging;
using Healthcare_hc;

namespace ExceptionsMid.Extenstions
{
    public static class ExceptionMiddlewareExtenstion
    {
        private static readonly HashSet<string> HandledExceptions = new()
        {
            typeof(ServiceValidationException).FullName,
            typeof(TazeezException).FullName,
            typeof(InvalidOperationException).FullName
        };

        public static void ConfigureExceptionHandler(this IApplicationBuilder app,
                                                    ILogger logger,
                                                    IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        try
                        {
                            await LogExceptionAsync(logger, context, contextFeature.Error)
                                        .AnyContext();
                        }
                        catch (Exception)
                        {
                        }

                        var exception = contextFeature.Error;
                        var errorCode = 0;

                        var responseText = env.IsDevelopment() || env.IsEnvironment("Local")
                                           ? exception?.Message
                                           : "Something went wrong";

                        if (HandledExceptions.Contains(exception.GetType().FullName))
                        {
                            int statusCode = (int)HttpStatusCode.BadRequest;

                            if (exception is TazeezException ex)
                            {
                                statusCode = ex.ErrorCode == 406 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
                                errorCode = ex.ErrorCode;
                            }

                            responseText = exception.Message;
                            context.Response.StatusCode = statusCode > 0 ?
                                                          statusCode :
                                                          (int)HttpStatusCode.BadRequest;
                        }

                        var error = new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            ErrorCode = errorCode,
                            Message = responseText
                        }.ToString();

                        await context.Response
                                     .WriteAsync(error, Encoding.UTF8)
                                     .AnyContext();
                    }
                });
            });
        }

        private static async Task LogExceptionAsync(ILogger logger, HttpContext context, Exception exception)
        {
            var sb = new StringBuilder();

            var logMessage = new LogMessage
            {
                LogLevel = LogEventLevel.Error,
                ApplicationName = typeof(Startup).Namespace
            };

            if (HandledExceptions.Contains(exception.GetType().FullName))
            {
                logMessage.LogLevel = LogEventLevel.Information;

                if (exception.GetType().FullName.Equals(typeof(InvalidOperationException).FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    logMessage.LogLevel = LogEventLevel.Fatal;
                }
            }

            while (exception != null)
            {
                sb.Append($"{exception.Message}{Environment.NewLine}StackTrace:{exception.StackTrace}");
                exception = exception.InnerException;
            }

            logMessage.Message = sb.ToString();

            if (context != null)
            {
                logMessage.RequestPath = context.Request?.Path;

                var helperManager = context.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                var ClaimId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (!string.IsNullOrWhiteSpace(ClaimId) && int.TryParse(ClaimId, out int id))
                {
                    var user = helperManager.GetUserRole(new UserModelView { Id = id });

                    if (user != null)
                    {
                        logMessage.UserId = user.Id;
                        logMessage.UserEmail = user.Email;
                    }
                }
            }

            await logger.LogMessageAsync(logMessage).AnyContext();
        }
    }
}
