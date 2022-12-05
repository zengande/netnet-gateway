using AutoMapper;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;

namespace NetNet.Gateway;

public class GatewayAutoMapperProfile : Profile
{
    public GatewayAutoMapperProfile()
    {
        CreateMap<ServiceCluster, GetServiceClusterRes>();
    }
}
