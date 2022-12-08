using NetNet.Gateway.Dtos.ServiceRoutes.Requests;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NetNet.Gateway;

public interface IServiceRouteAppService : IApplicationService
{
    Task<PagedResultDto<QueryServiceRouteRes>> QueryAsync(QueryServiceRouteReq req);
    Task<GetServiceRouteRes> GetAsync(Guid id);
    Task<Guid> CreateAsync(InputServiceRouteReq req);
    Task<bool> UpdateAsync(Guid id, InputServiceRouteReq req);
    Task<bool> DeleteAsync(Guid id);
}
