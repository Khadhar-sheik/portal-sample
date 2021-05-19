using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Org.Common.Web.Configuration;

namespace Org.Common.Web.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, string[] args)
        {
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentConstants.ServiceDiscovery)))
            {
                builder.Add(new ServiceDiscoveryConfigurationSource());
                return builder;
            }

            builder.AddEnvironmentVariables();

            foreach (var jsonFile in Directory.GetFiles("./Config/", "*.json"))
            {
                builder.AddJsonFile(jsonFile);
            }

            builder.AddJsonFile("appsettings.json", true);

            if (args != null)
            {
                builder.AddCommandLine(args);
            }

            return builder;
        }
    }
}
