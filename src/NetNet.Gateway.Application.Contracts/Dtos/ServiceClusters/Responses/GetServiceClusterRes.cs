using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class GetServiceClusterRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public List<ServiceDestinationRes> Destinations { get; set; }

    /// <summary>
    /// 健康检测配置
    /// </summary>
    public ServiceClusterHealthCheckConfigDto? HealthCheckConfig { get; set; } = new();

    public ServiceClusterHttpClientConfigDto? HttpClientConfig { get; set; } = new();

    public ServiceClusterHttpRequestConfigDto? HttpRequestConfig { get; set; } = new();
}
