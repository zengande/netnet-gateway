using NetNet.Gateway.Events.ServiceClusters;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

/// <summary>
/// 服务集群
/// </summary>
public sealed class ServiceCluster : AuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string? LoadBalancingPolicy { get; private set; }

    /// <summary>
    /// 请求配置
    /// </summary>
    public ServiceClusterHttpRequestConfig? HttpRequestConfig { get; private set; }

    /// <summary>
    /// HttpClient 配置
    /// </summary>
    public ServiceClusterHttpClientConfig? HttpClientConfig { get; private set; }

    /// <summary>
    /// 健康检测配置
    /// </summary>
    public ServiceClusterHealthCheckConfig? HealthCheckConfig { get; private set; }

    /// <summary>
    /// 服务目的地
    /// </summary>
    public IReadOnlyCollection<ServiceDestination> Destinations => _destinations;

    private readonly List<ServiceDestination> _destinations;

    private ServiceCluster()
    {
        _destinations = new();
    }

    public ServiceCluster(string name, string? loadBalancingPolicy, ServiceClusterHttpRequestConfig httpRequestConfig,
        ServiceClusterHttpClientConfig httpClientConfig, ServiceClusterHealthCheckConfig healthCheckConfig, List<ServiceDestination> destinations)
        : this()
    {
        Name = name;
        LoadBalancingPolicy = loadBalancingPolicy;
        HttpRequestConfig = httpRequestConfig;
        HttpClientConfig = httpClientConfig;
        HealthCheckConfig = healthCheckConfig;
        _destinations = destinations;

        // 新增服务后通知配置改变
        AddLocalEvent(new ServiceClusterChangedDomainEvent());
    }


    public void Update(string name, string? loadBalancingPolicy, ServiceClusterHttpRequestConfig httpRequestConfig,
        ServiceClusterHttpClientConfig httpClientConfig, ServiceClusterHealthCheckConfig healthCheckConfig, List<ServiceDestination> destinations)
    {
        Name = name;
        LoadBalancingPolicy = loadBalancingPolicy;
        HttpRequestConfig = httpRequestConfig;
        HttpClientConfig = httpClientConfig;
        if (HealthCheckConfig is null)
        {
            HealthCheckConfig = new(healthCheckConfig.AvailableDestinationsPolicy, healthCheckConfig.Active, healthCheckConfig.Passive);
        }
        else
        {
            HealthCheckConfig.Update(healthCheckConfig.AvailableDestinationsPolicy, healthCheckConfig.Active, healthCheckConfig.Passive);
        }

        // 不存在的移除掉
        var removedIds = _destinations.Select(x => x.Id)
            .Except(destinations.Select(x => x.Id));
        _destinations.RemoveAll(x => removedIds.Contains(x.Id));
        foreach (var item in destinations)
        {
            var destination = _destinations.FirstOrDefault(x => item.Id != Guid.Empty && x.Id == item.Id);
            // 新增
            if (destination is null)
            {
                _destinations.Add(item);
                continue;
            }

            destination.Update(item.Key, item.Address, item.Health, item.Metadata);
        }

        // 新增服务后通知配置改变
        AddLocalEvent(new ServiceClusterChangedDomainEvent());
    }

    public ClusterConfig ToYarpClusterConfig() => new()
    {
        ClusterId = Id.ToString(),
        LoadBalancingPolicy = LoadBalancingPolicy,
        Destinations = _destinations.ToDictionary(x => x.Id.ToString(),
            x => new DestinationConfig { Address = x.Address, Health = x.Health, Metadata = x.Metadata }),
        HttpClient = HttpClientConfig?.ToYarpHttpClientConfig(),
        HttpRequest = HttpRequestConfig?.ToYarpForwarderRequestConfig(),
        HealthCheck = HealthCheckConfig?.ToYarpHealthCheckConfig()
    };
}
