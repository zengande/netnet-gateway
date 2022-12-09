using NetNet.Gateway.Extensions;
using Volo.Abp.Domain.Entities;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

public sealed class ServiceClusterHealthCheckConfig : Entity<Guid>
{
    public Guid ServiceClusterId { get; private set; }

    /// <summary>
    /// 可用终点策略
    /// </summary>
    public string? AvailableDestinationsPolicy { get; protected set; }

    /// <summary>
    /// 主动健康检查
    /// </summary>
    public ServiceClusterActiveHealthCheckConfig Active { get; protected set; }

    /// <summary>
    /// 被动健康检查
    /// </summary>
    public ServiceClusterPassiveHealthCheckConfig Passive { get; private set; }

    private ServiceClusterHealthCheckConfig()
    {
        Passive = new();
        Active = new();
    }

    public ServiceClusterHealthCheckConfig(string? availableDestinationsPolicy, ServiceClusterActiveHealthCheckConfig active,
        ServiceClusterPassiveHealthCheckConfig passive)
    {
        AvailableDestinationsPolicy = availableDestinationsPolicy;
        Active = active;
        Passive = passive;
    }

    public void Update(string? availableDestinationsPolicy, ServiceClusterActiveHealthCheckConfig active,
        ServiceClusterPassiveHealthCheckConfig passive)
    {
        AvailableDestinationsPolicy = availableDestinationsPolicy;
        Active = active;
        Passive = passive;
    }

    public HealthCheckConfig ToYarpHealthCheckConfig() => new()
    {
        AvailableDestinationsPolicy = AvailableDestinationsPolicy,
        Active = new()
        {
            Enabled = Active.Enabled,
            Interval = Active.IntervalSeconds.ToTimeSpan(),
            Path = Active.Path,
            Policy = Active.Policy,
            Timeout = Active.TimeoutSeconds.ToTimeSpan()
        },
        Passive = new()
        {
            Enabled = Passive.Enabled, Policy = Passive.Policy, ReactivationPeriod = Passive.ReactivationPeriodSeconds.ToTimeSpan()
        }
    };
}
