using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Org.Common.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T BindRoot<T>(this IConfiguration configuration, T options = null)
            where T : class, new()
        {
            if (options == null)
                options = new T();

            configuration = configuration.GetSection(typeof(T).Name);
            configuration.Bind(options);

            return options;
        }
    }
}
