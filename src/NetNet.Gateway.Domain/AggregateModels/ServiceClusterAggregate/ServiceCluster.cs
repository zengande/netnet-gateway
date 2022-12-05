using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

/// <summary>
/// 服务集群
/// </summary>
public sealed class ServiceCluster : AuditedAggregateRoot<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; private set; }

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
    /// 服务目的地
    /// </summary>
    public IReadOnlyCollection<ServiceDestination> Destinations => _destinations;

    private readonly List<ServiceDestination> _destinations;

    private ServiceCluster()
    {
        _destinations = new();
    }

    public ServiceCluster(string name, string description, string? loadBalancingPolicy, ServiceClusterHttpRequestConfig? httpRequestConfig,
        ServiceClusterHttpClientConfig? httpClientConfig)
        : this()
    {
        Name = name;
        Description = description;
        LoadBalancingPolicy = loadBalancingPolicy;
        HttpRequestConfig = httpRequestConfig;
        HttpClientConfig = httpClientConfig;
    }

    public void AddDestination(string key, string address, string health, Dictionary<string, string> metadata)
    {
        if (_destinations.Any(x => x.Key == key))
        {
            throw new UserFriendlyException($"已存在{key}");
        }

        var destination = new ServiceDestination(Id, key, address, health, metadata);
        _destinations.Add(destination);
    }

    public ClusterConfig ToYarpClusterConfig() => new()
    {
        ClusterId = Name,
        LoadBalancingPolicy = LoadBalancingPolicy,
        Destinations = _destinations.ToDictionary(x => x.Key,
            x => new DestinationConfig { Address = x.Address, Health = x.Health, Metadata = x.Metadata }),
        HttpClient = HttpClientConfig?.ToYarpHttpClientConfig(),
        HttpRequest = HttpRequestConfig?.ToYarpForwarderRequestConfig()
    };
}
