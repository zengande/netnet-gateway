using System.ComponentModel.DataAnnotations;

namespace NetNet.Gateway.Dtos.ServiceClusters.Requests;

public class InputServiceClusterReq
{
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 负载均衡策略
    /// </summary>
    public string LoadBalancingPolicy { get; set; }

    public string Description { get; set; }
}
