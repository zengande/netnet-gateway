using Volo.Abp.Domain.Values;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

/// <summary>
/// 被动健康检查
/// </summary>
public class ServiceClusterPassiveHealthCheckConfig : ValueObject
{
    /// <summary>Whether passive health checks are enabled.</summary>
    public bool? Enabled { get; set; }

    /// <summary>Passive health check policy.</summary>
    public string? Policy { get; set; }

    /// <summary>
    /// Destination reactivation period after which an unhealthy destination is considered healthy again.
    /// </summary>
    public int? ReactivationPeriodSeconds { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Enabled;
        yield return Policy;
        yield return ReactivationPeriodSeconds;
    }
}
