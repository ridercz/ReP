using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Altairis.ReP.Web;

public class SetupCompleteHealthCheck : IHealthCheck {
    private readonly RepDbContext dc;

    public SetupCompleteHealthCheck(RepDbContext dc) {
        this.dc = dc;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
        var setupComplete = await this.dc.Users.AnyAsync(cancellationToken);
        return setupComplete ? HealthCheckResult.Healthy() : HealthCheckResult.Degraded("Initial setup not complete.");
    }
}