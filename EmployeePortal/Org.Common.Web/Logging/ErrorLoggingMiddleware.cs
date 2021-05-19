using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Org.Common.Web.Logging
{
    public class ErrorLoggingMiddleware
    {
        private readonly LoggingOptions _options;
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ErrorLoggingMiddleware(IOptions<LoggingOptions> options, RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            try
            {
                await _next(context);
            }
            catch (System.Exception e)
            {
                var logModel = new RequestLogModel(context)
                {
                    AppZone = _options.AppZone,
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow
                };

                if (context.Request.Body?.CanSeek ?? false)
                {
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    logModel.RequestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                }

                _loggerFactory.CreateLogger<ErrorLoggingMiddleware>().LogError(e, "{@request}", logModel);

                throw;
            }
        }
    }
}
