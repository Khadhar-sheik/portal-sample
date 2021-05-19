using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Org.Common.Web.Extensions;
using Org.Common.Web.Logging;

namespace Org.Common.Web
{
    public abstract class WebHost<TService> where TService : WebHost<TService>
    {
        protected IConfiguration Configuration { get; private set; }

        protected static async Task Start(string[] args)
        {
            LoggerConfigurationUtils.SetInitialLoggingConfiguration();

            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (System.Exception e)
            {
                LogManager.GetCurrentClassLogger().Fatal(e);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        protected WebHost(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder();

            builder.ConfigureAppConfiguration(b => b.AddDefaults(args));
            builder.ConfigureServices((context, services) =>
            {
                LoggerConfigurationUtils.ConfigureLogging(context.Configuration);
            });
            builder.ConfigureLogging(b =>
            {
                b.ClearProviders();
                b.AddNLog();
            });

            builder.ConfigureServices((context, services) =>
            {
                //Configuration = context.Configuration;
                //HostingEnvironment = context.HostingEnvironment;

                services.ConfigureRoot();
            });

            //builder.ConfigureServices(ConfigureServices);

            return builder.UseStartup<TService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorLoggingMiddleware>();

            ConfigureApp(app, env);
        }

        public abstract void ConfigureServices(IServiceCollection services);
        protected abstract void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
