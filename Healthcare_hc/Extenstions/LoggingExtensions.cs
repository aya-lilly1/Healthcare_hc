using HealthCare_Common.Extensions;
using HealthCare_Common.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Tazeez.Common.Extensions
{
    public static class LoggingExtensions
    {
        public static async Task LogExceptionAsync(this ILogger logger, Exception exception)
        {
            var sb = new StringBuilder();

            while (exception != null)
            {
                sb.Append($"Message: {exception.Message} {Environment.NewLine}StackTrace: {exception.StackTrace}");
                exception = exception.InnerException;
            }

            logger.Error(sb.ToString());

            await Task.CompletedTask;
        }

        public static async Task LogMessageAsync(this ILogger logger, LogMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Message))
                return;

            var log = $@"LogLeveL : {message.LogLevel}{Environment.NewLine}
                         Log Date: {message.CreatedOn}{Environment.NewLine}
                         UserId: {message.UserId}{Environment.NewLine}
                         User Email: {message.UserEmail}{Environment.NewLine}
                         Request Path: {message.RequestPath}{Environment.NewLine}
                         Application Name: {message.ApplicationName}{Environment.NewLine}
                         Message: {message.Message}{Environment.NewLine}"
                        .StripLeadingWhitespace();

            switch (message.LogLevel)
            {
                case LogEventLevel.Debug:
                    logger.Debug(log);
                    break;

                case LogEventLevel.Information:
                    logger.Information(log);
                    break;

                case LogEventLevel.Warning:
                    logger.Warning(log);
                    break;

                case LogEventLevel.Error:
                    logger.Error(log);
                    break;

                case LogEventLevel.Fatal:
                    logger.Fatal(log);
                    break;

                default:
                    break;
            }

            await Task.CompletedTask.AnyContext();
        }
    }
}