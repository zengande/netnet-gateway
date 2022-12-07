using Volo.Abp.Domain.Entities.Auditing;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

/// <summary>
/// 服务路由
/// </summary>
public sealed class ServiceRoute : AuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 服务id
    /// </summary>
    public Guid ServiceClusterId { get; private set; }

    /// <summary>
    /// 授权策略
    /// </summary>
    public string? AuthorizationPolicy { get; private set; }

    /// <summary>
    /// 跨域策略
    /// </summary>
    public string? CrosPolicy { get; private set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? Order { get; private set; }

    /// <summary>
    /// 匹配规格
    /// </summary>
    public ServiceRouteMatch? Match { get; private set; }

    // /// <summary>
    // /// 请求转换
    // /// </summary>
    // public IList<ServiceRouteTransform> Transforms { get; private set; }

    private ServiceRoute()
    {
        // Transforms = new List<ServiceRouteTransform>();
    }

    public ServiceRoute(string name, Guid serviceClusterId, string? authorizationPolicy, string? crosPolicy, int? order)
        : this()
    {
        Name = name;
        ServiceClusterId = serviceClusterId;
        AuthorizationPolicy = authorizationPolicy;
        CrosPolicy = crosPolicy;
        Order = order;
    }

    public void SetMatchRule(string? hosts, string? methods, string? path)
    {
        Match ??= new(Id, hosts, methods, path);

    }
}
