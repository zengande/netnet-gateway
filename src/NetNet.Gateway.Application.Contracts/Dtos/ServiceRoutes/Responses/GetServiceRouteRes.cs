using Volo.Abp.Application.Dtos;

namespace NetNet.Gateway.Dtos.ServiceRoutes.Responses;

public class GetServiceRouteRes : EntityDto<Guid>
{
    public string Name { get; set; }
}
