using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Services;

public class ServiceRouteAppService : GatewayAppService, IServiceRouteAppService
{
    public async Task<PagedResultDto<QueryServiceRouteRes>> QueryAsync(QueryServiceRouteReq req)
    {
        var count = 100;
        var items = Enumerable.Range(0, req.MaxResultCount)
            .Select(x => new QueryServiceRouteRes()
            {
                Id = Guid.NewGuid(), Name = "Route_" + x, ServiceClusterName = "Service_" + x % 2, Path = "/api/{**remind}"
            }).ToList();


        return new(count, items);
    }

    public async Task<GetServiceRouteRes> GetAsync(Guid id)
    {
        return new(){Name = "Test Route"};
    }

    public async Task<Guid> CreateAsync(InputServiceRouteReq req)
    {
        return Guid.Empty;
    }

    public async Task<bool> UpdateAsync(Guid id, InputServiceRouteReq req)
    {
        return true;
    }
}
