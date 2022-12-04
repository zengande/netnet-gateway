using Volo.Abp.Domain.Entities;

namespace NetNet.Gateway.Entities;

public class HealthCheckOptions : Entity<long>
{
    /// <summary>
    /// Passive health check options.
    /// </summary>
    public virtual PassiveHealthCheckOptions Passive { get; init; }

    /// <summary>
    /// Active health check options.
    /// </summary>
    public virtual ActiveHealthCheckOptions Active { get; init; }

    /// <summary>
    /// Available destinations policy.
    /// </summary>
    public string? AvailableDestinationsPolicy { get; init; }

    public string ClusterId { get; set; }
    public virtual Cluster Cluster { get; set; }
}
