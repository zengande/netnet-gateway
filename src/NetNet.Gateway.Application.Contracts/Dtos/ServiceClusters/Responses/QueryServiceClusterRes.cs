using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceClusters.Responses;

public class QueryServiceClusterRes : EntityDto<long>
{
    public string Name { get; set; }

    public string Source { get; set; }

    public List<QueryServiceInfoRes> Services { get; set; } = new();
}
