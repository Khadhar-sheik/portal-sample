using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Org.Common.Web.Extensions
{
    public static class OptionsConfigurationExtensions
    {
        public static IServiceCollection ConfigureRoot(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.TryAdd(ServiceDescriptor.Describe(typeof(IConfigureOptions<>), typeof(ConfigureOptions<>), ServiceLifetime.Transient));

            return serviceCollection;
        }

        private class ConfigureOptions<TOptions> : IConfigureOptions<TOptions> where TOptions: class, new()
        {
            private readonly IConfiguration _configuration;

            public ConfigureOptions(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public void Configure(TOptions options)
            {
                _configuration.BindRoot(options);
            }
        }
    }
}
