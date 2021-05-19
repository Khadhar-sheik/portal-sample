using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Org.Common.Web.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return HealthCheckResult.Healthy("Ping HealthCheck passed");
        }
    }
}
