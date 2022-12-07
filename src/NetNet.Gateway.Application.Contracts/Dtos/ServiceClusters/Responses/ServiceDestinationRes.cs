using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class ServiceDestinationRes : EntityDto<Guid>
{
    public string Key { get; set; }

    public string Address { get; set; }

    public string Health { get; set; }

    public Dictionary<string, string> Metadata { get; set; }
}
