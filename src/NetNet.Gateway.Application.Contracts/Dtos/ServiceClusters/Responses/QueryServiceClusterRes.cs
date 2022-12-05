using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class QueryServiceClusterRes : EntityDto<long>
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
    /// 目的地数量
    /// </summary>
    public int DestinationCount { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }
}
