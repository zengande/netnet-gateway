namespace NetNet.Gateway.Dtos.ServiceClusters;

public class ServiceClusterPassiveHealthCheckConfigDto
{
    /// <summary>Whether passive health checks are enabled.</summary>
    public bool? Enabled { get; set; }

    /// <summary>Passive health check policy.</summary>
    public string? Policy { get; set; }

    /// <summary>
    /// Destination reactivation period after which an unhealthy destination is considered healthy again.
    /// </summary>
    public int? ReactivationPeriodSeconds { get; set; }
}
