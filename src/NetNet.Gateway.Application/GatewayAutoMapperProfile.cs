using AutoMapper;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;

namespace NetNet.Gateway;

public class GatewayAutoMapperProfile : Profile
{
    public GatewayAutoMapperProfile()
    {
        CreateMap<ServiceCluster, GetServiceClusterRes>();
        CreateMap<ServiceDestination, ServiceDestinationRes>();

        CreateMap<ServiceRoute, GetServiceRouteRes>();
    }
}
