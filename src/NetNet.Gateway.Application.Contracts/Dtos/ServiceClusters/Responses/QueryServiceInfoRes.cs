using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class QueryServiceInfoRes : EntityDto<long>
{
    public string Version { get; set; }

    public string Address { get; set; }

    public string Health { get; set; }
}
