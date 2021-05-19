using System.Threading.Tasks;
using EmployeePortal.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.Common.Domain;
using Org.Common.Web;
using Org.Common.Web.Extensions;
using Org.DAL.MySql;
using Org.Common.Web.Middleware;
using Org.Common.Web.Options;
using Org.Services;
using Microsoft.AspNetCore.Identity;


namespace EmployeePortal
{
    public class ApiService : WebHost<ApiService>
    {
        public static async Task Main(string[] args)
        {
            await Start(args);
        }

        public ApiService(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            // In production, the Angular files will be served from this directory
           // services.AddSpaStaticFiles(configuration =>
           /// {
           //     configuration.RootPath = "ClientApp/dist";
          //  });

            services.AddAuth();
            
            services.RegisterDal(Configuration);
            services.RegisterServices();
            

            services.AddIdentityCore<EmployeePortalUser>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 6;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireLowercase = false;
            })
                .AddSignInManager()
                .AddRoles<IdentityRole>()
                .AddIdentityEfStores();
            services.AddCors();
        }

        protected override void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
          //  var corsOptions = Configuration.BindRoot(new CorsOptions());
           // if (corsOptions.Enabled)
          //  {
              //  app.UseCors(builder =>
            //   builder.WithOrigins("https: //localhost:44360").AllowAnyHeader().AllowAnyMethod());
            //}
            

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseForwardedHeaders();
            //app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3(s => { s.DocumentPath = "/swagger/{documentName}/swagger.json"; });
            }

            var corsOptions = Configuration.BindRoot(new CorsOptions());
            if (corsOptions.Enabled)
            {
                app.UseCors(b =>
                {
                    b.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(string.IsNullOrWhiteSpace(corsOptions.AllowedHosts) ? new string[] { "*" } : corsOptions.AllowedHosts.Split(';'));
                      
                });

            }


            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseEndpoints(c =>
            {
                //c.MapHealthChecks("/health", new HealthCheckOptions
                //{
                //    AllowCachingResponses = false,
                //    ResponseWriter = HealthCheckResponseWriter.WriteHealthCheckResponse
                //});
                c.MapDefaultControllerRoute();
            });
        }

        private void RegisterDal(IServiceCollection services)
        {
            //services.AddSingleton<IUserDataProvider, UserDataProvider>();
        }

        private void RegisterHealthChecks(IServiceCollection services)
        {
            // services.AddHealthChecks()
            //     .AddCheck<ApiHealthCheck>("api")
            //     .AddMySql(Configuration.GetConnectionString(MySqlConnectionFactory.CONNECTION_STRING_NAME), MySqlConnectionFactory.CONNECTION_STRING_NAME);
        }
    }
   
}