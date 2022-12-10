using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class QueryServiceClusterRes : EntityDto<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string? LoadBalancePolicy { get; set; }

    /// <summary>
    /// 终点数量
    /// </summary>
    public int DestinationCount { get; set; }

    /// <summary>
    /// 路由数量
    /// </summary>
    public int RouteCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 终点
    /// </summary>
    public List<ServiceDestinationRes> Destinations { get; set; }
}
