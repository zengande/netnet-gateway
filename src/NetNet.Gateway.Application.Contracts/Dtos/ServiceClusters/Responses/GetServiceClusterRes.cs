using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class GetServiceClusterRes : EntityDto<long>
{
    public string Name { get; set; }
    public string Description { get; set; }
}
