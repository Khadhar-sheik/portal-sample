using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.StructuredLogging.Json;
using NLog.Targets;
using NLog.Targets.ElasticSearch;
using NLog.Targets.Wrappers;

namespace Org.Common.Web.Logging
{
    public static class LoggerConfigurationUtils
    {
        private const int FlushTimeout = 1000;

        public static void SetInitialLoggingConfiguration()
        {
            var configuration = new LoggingConfiguration();
            configuration.AddRuleForAllLevels(new ColoredConsoleTarget());

            LogManager.Configuration = configuration;
        }

        public static void ConfigureLogging(IConfiguration appConfig)
        {
            var loggingOptions = new LoggingOptions();
            appConfig.GetSection(nameof(LoggingOptions)).Bind(loggingOptions);

            var configuration = new LoggingConfiguration();

            configuration.AddRuleForAllLevels(new ColoredConsoleTarget());

            var errorLoggerElkTarget = new ElasticSearchTarget
            {
                Uri = loggingOptions.ElasticSearchUri,
                Index = new SimpleLayout($"{loggingOptions.ErrorLoggingIndex}-${{date:format=yyyy.MM.dd}}"),
                IncludeAllProperties = true,
                Layout = new JsonLayout()
            };
            configuration.AddRule(LogLevel.Error,LogLevel.Fatal, new BufferingTargetWrapper
            {
                WrappedTarget = errorLoggerElkTarget,
                FlushTimeout = FlushTimeout
            });

            if (loggingOptions.RequestLoggingEnabled)
            {
                var requestLoggingElkTarget = new ElasticSearchTarget
                {
                    Uri = loggingOptions.ElasticSearchUri,
                    Index = new SimpleLayout($"{loggingOptions.RequestLoggingIndex}-${{date:format=yyyy.MM.dd}}"),
                    IncludeAllProperties = true,
                    Layout = new JsonLayout()
                };
                configuration.AddRuleForAllLevels(new BufferingTargetWrapper
                {
                    WrappedTarget = requestLoggingElkTarget,
                    FlushTimeout = FlushTimeout
                }, $"*{nameof(RequestLoggingMiddleware)}");
            }

            LogManager.Configuration = configuration;
        }
    }
}
