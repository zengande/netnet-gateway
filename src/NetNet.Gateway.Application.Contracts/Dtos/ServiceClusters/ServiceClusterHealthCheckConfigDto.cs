using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters;

public class ServiceClusterHealthCheckConfigDto : EntityDto<Guid>
{
    /// <summary>
    /// 可用终点策略
    /// </summary>
    public string? AvailableDestinationsPolicy { get; set; }

    /// <summary>
    /// 主动健康检查
    /// </summary>
    public ServiceClusterActiveHealthCheckConfigDto Active { get; set; } = new();

    /// <summary>
    /// 被动健康检查
    /// </summary>
    public ServiceClusterPassiveHealthCheckConfigDto Passive { get; set; } = new();
}
