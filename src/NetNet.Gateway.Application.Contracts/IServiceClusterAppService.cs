using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NetNet.Gateway;

public interface IServiceClusterAppService : IApplicationService
{
    Task<PagedResultDto<QueryServiceClusterRes>> QueryAsync(QueryServiceClusterReq req);
}
