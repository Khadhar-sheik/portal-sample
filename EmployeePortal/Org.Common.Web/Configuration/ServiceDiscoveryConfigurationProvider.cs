using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;

namespace Org.Common.Web.Configuration
{
    public class ServiceDiscoveryConfigurationProvider : ConfigurationProvider
    {
        private Action<ServiceDiscoveryConfigurationProviderOptions> _optionsBuilder;
        private ILogger<ServiceDiscoveryConfigurationProvider> _logger;

        public ServiceDiscoveryConfigurationProvider(Action<ServiceDiscoveryConfigurationProviderOptions> optionsBuilder)
        {
            _optionsBuilder = optionsBuilder;
        }

        public override void Load()
        {
            var options = new ServiceDiscoveryConfigurationProviderOptions();
            _optionsBuilder(options);

            var serviceDiscoveryUrl = Environment.GetEnvironmentVariable(EnvironmentConstants.ServiceDiscovery);
            if (string.IsNullOrWhiteSpace(serviceDiscoveryUrl))
            {
                return;
            }

            try
            {
                var response = new HttpClient().GetAsync($"{serviceDiscoveryUrl}/api/settings/{options.ServiceName}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
                
                foreach (var setting in settings)
                {
                    Data.Add(setting.Key, setting.Value);
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Error loading configuration from service discovery");
                throw;
            }
        }
    }
}
