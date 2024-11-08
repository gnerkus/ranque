using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Contracts
{
    public interface IHealthCheck
    {
        Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken
            cancellationToken = default);
    }
}