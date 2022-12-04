using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Services;

public class ServiceClusterAppService : GatewayAppService, IServiceClusterAppService
{
    public async Task<PagedResultDto<QueryServiceClusterRes>> QueryAsync(QueryServiceClusterReq req)
    {


        return new();
    }
}
