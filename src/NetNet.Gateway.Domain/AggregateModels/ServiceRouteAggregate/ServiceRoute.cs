using NetNet.Gateway.Events.ServiceRoutes;
using NetNet.Gateway.Extensions;
using NetNet.Gateway.SeedWork;
using Volo.Abp.Domain.Entities.Auditing;
using Yarp.ReverseProxy.Configuration;

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
    public ServiceRouteMatch Match { get; private set; }

    /// <summary>
    /// 请求转换
    /// </summary>
    public IList<ServiceRouteTransform> Transforms { get; private set; }

    public Metadata Metadata { get; private set; }

    private ServiceRoute()
    {
        Transforms = new List<ServiceRouteTransform>();
        Metadata = new();
    }

    public ServiceRoute(string name, Guid serviceClusterId, string? authorizationPolicy, string? crosPolicy, int? order,
        ServiceRouteMatch match, List<ServiceRouteTransform> transforms)
        : this()
    {
        Name = name;
        ServiceClusterId = serviceClusterId;
        AuthorizationPolicy = authorizationPolicy;
        CrosPolicy = crosPolicy;
        Order = order;
        Match = match;
        Transforms = transforms;

        // 新增服务后通知配置改变
        AddLocalEvent(new ServiceRouteChangedDomainEvent());
    }

    public void Update(string name, Guid serviceClusterId, string? authorizationPolicy, string? crosPolicy, int? order,
        ServiceRouteMatch match, List<ServiceRouteTransform> transforms)
    {
        Name = name;
        ServiceClusterId = serviceClusterId;
        AuthorizationPolicy = authorizationPolicy;
        CrosPolicy = crosPolicy;
        Order = order;
        Transforms = transforms;

        Match.Update(match.Hosts, match.Methods, match.Path);

        // 通知配置改变
        AddLocalEvent(new ServiceRouteChangedDomainEvent());
    }

    public RouteConfig ToYarpRouteConfig() => new()
    {
        RouteId = Id.ToString(),
        AuthorizationPolicy = AuthorizationPolicy,
        CorsPolicy = CrosPolicy,
        ClusterId = ServiceClusterId.ToString(),
        Order = Order,
        Match = Match.ToYarpRouteMatch(),
        Transforms = Transforms
            .GroupBy(x => x.GroupIndex)
            .OrderBy(x => x.Key)
            .Select(x => x.ToDictionary(y => y.Key, y => y.Value))
            .ToList(),
        Metadata = (Dictionary<string, string>)Metadata
    };
}
