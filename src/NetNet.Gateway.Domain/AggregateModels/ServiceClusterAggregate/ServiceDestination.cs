using Volo.Abp.Domain.Entities.Auditing;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

/// <summary>
/// 服务目的地
/// </summary>
public sealed class ServiceDestination : AuditedEntity<long>
{
    /// <summary>
    /// 服务集群id
    /// </summary>
    public long ServiceClusterId { get; private set; }

    /// <summary>
    /// key
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// 健康检查地址
    /// </summary>
    public string Health { get; private set; }

    /// <summary>
    /// 元数据
    /// </summary>
    public Dictionary<string, string> Metadata { get; private set; }

    private ServiceDestination() { }

    public ServiceDestination(long serviceClusterId, string key, string address, string health, Dictionary<string, string> metadata)
        : this()
    {
        ServiceClusterId = serviceClusterId;
        Key = key;
        Address = address;
        Health = health;
        Metadata = metadata;
    }
}
