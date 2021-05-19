using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Org.Common.Web.Configuration
{
    public class ServiceDiscoveryConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceDiscoveryConfigurationProvider(o => o.ServiceName = Environment.GetEnvironmentVariable(EnvironmentConstants.ServiceName));
        }
    }
}
