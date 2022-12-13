using System.ComponentModel.DataAnnotations;

namespace NetNet.Gateway.Dtos.ServiceClusters.Requests;

public class InputServiceClusterReq
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string LoadBalancingPolicy { get; set; }

    /// <summary>
    /// 服务终点
    /// </summary>
    public IEnumerable<InputServiceDestinationReq> Destinations { get; set; } = new List<InputServiceDestinationReq>();

    /// <summary>
    /// 健康检测配置
    /// </summary>
    public ServiceClusterHealthCheckConfigDto HealthCheckConfig { get; set; } = new();

    /// <summary>
    /// http 请求配置
    /// </summary>
    public ServiceClusterHttpRequestConfigDto HttpRequestConfig { get; set; } = new();

    /// <summary>
    /// httpclient 配置
    /// </summary>
    public ServiceClusterHttpClientConfigDto HttpClientConfig { get; set; } = new();

    /// <summary>
    /// Swagger 配置
    /// </summary>
    public ServiceClusterSwaggerConfig SwaggerConfig { get; set; } = new();
}
