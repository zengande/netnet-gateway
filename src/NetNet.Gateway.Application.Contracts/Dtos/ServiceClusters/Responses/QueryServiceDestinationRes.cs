using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class QueryServiceDestinationRes : EntityDto<long>
{
    public string Key { get; set; }

    public string Address { get; set; }

    public string Health { get; set; }
}
