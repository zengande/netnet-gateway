using Volo.Abp.Domain.Entities.Auditing;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

/// <summary>
/// 服务路由
/// </summary>
public class ServiceRoute : AuditedAggregateRoot<Guid>
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

    /// <summary>
    /// 请求转换
    /// </summary>
    public IReadOnlyList<IReadOnlyDictionary<string, string>> Transforms { get; private set; }

    private ServiceRoute()
    {
        Transforms = new List<Dictionary<string, string>>();
    }
}
