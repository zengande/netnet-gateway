using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class GetServiceClusterRes : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public List<ServiceDestinationRes> Destinations { get; set; }
}
