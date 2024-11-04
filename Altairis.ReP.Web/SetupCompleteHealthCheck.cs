using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Altairis.ReP.Web;

public class SetupCompleteHealthCheck(RepDbContext dc) : IHealthCheck {

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
        var setupComplete = await dc.Users.AnyAsync(cancellationToken);
        return setupComplete ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded("Initial setup not complete.");
    }
}