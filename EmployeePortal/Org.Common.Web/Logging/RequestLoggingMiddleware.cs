using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.StructuredLogging.Json;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Org.Common.Web.Logging
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LoggingOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public const string ResponseIdHeaderName = "X-Request-Id";

        public RequestLoggingMiddleware(RequestDelegate next, IOptions<LoggingOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid();
            context.Response.Headers.Append(ResponseIdHeaderName, requestId.ToString());

            var startTime = DateTime.UtcNow;
            var originalBody = context.Response.Body;

            await using var responseRewindStream = new MemoryStream();
            await using var requestRewindStream = new MemoryStream();

            try
            {
                if (context.Request.Body != null && context.Request.Body.CanRead)
                {
                    await context.Request.Body?.CopyToAsync(requestRewindStream);
                    requestRewindStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = requestRewindStream;
                }

                context.Response.Body = responseRewindStream;
                await _next(context);
                responseRewindStream.Position = 0;
                await responseRewindStream.CopyToAsync(originalBody);
            }
            finally
            {
                context.Response.Body = originalBody;
            }

            var logModel = new RequestLogModel(context)
            {
                AppZone = _options.AppZone,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,
            };

            MediaTypeHeaderValue.TryParse(context.Response.ContentType, out var responseType);
            if (responseType?.MediaType == "application/json")
            {
                responseRewindStream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(responseRewindStream);
                logModel.ResponseBody = await reader.ReadToEndAsync();
            }

            if (context.Request.ContentType?.Equals("application/json", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                requestRewindStream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(requestRewindStream);
                logModel.RequestBody = await reader.ReadToEndAsync();
            }

            _loggerFactory.CreateLogger<RequestLoggingMiddleware>().LogInformation("{@request}", logModel);

        }
    }
}
