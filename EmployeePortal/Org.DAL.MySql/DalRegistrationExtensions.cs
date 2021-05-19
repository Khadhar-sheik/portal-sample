using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.Common.DataProvider;
using Org.DAL.MySql.DataProvider;

[assembly:InternalsVisibleTo("EmployeePortal.Tests")]
namespace Org.DAL.MySql
{
    public static class DalRegistrationExtensions
    {
        private const string CONNECTION_STRING_NAME = "EmpoyeePortalConnection";
        public static void RegisterDal(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("EmpoyeePortalConnection");
            
            services.AddDbContext<EmployeContext>(o =>
            {
                o.UseMySql("server=localhost;database=EmployePortal-Migration;allowuservariables=True;user id=root;password=5656");
            });

            services.AddScoped<IDatabaseMigrationProvider, DatabaseMigrationProvider>();
            services.AddScoped<ILeaveDataProvider, LeaveDataProvider>();
        }

        public static void AddIdentityEfStores(this IdentityBuilder identityBuilder)
        {
            identityBuilder.AddEntityFrameworkStores<EmployeContext>();
        }
    }
}