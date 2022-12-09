using AutoMapper;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using NetNet.Gateway.Dtos.ServiceClusters;
using NetNet.Gateway.Dtos.ServiceClusters.Requests;
using NetNet.Gateway.Dtos.ServiceClusters.Responses;
using NetNet.Gateway.Dtos.ServiceRoutes;
using NetNet.Gateway.Dtos.ServiceRoutes.Responses;
using NetNet.Gateway.Extensions;

namespace NetNet.Gateway;

public class GatewayAutoMapperProfile : Profile
{
    public GatewayAutoMapperProfile()
    {
        CreateMap<ServiceCluster, GetServiceClusterRes>();
        CreateMap<ServiceDestination, ServiceDestinationRes>();
        CreateMap<InputServiceDestinationReq, ServiceDestination>();
        CreateMap<ServiceClusterHttpRequestConfig, ServiceClusterHttpRequestConfigDto>()
            .ReverseMap();
        CreateMap<ServiceClusterHttpClientConfig, ServiceClusterHttpClientConfigDto>()
            .ReverseMap();
        CreateMap<ServiceClusterHealthCheckConfig, ServiceClusterHealthCheckConfigDto>()
            .ReverseMap();
        CreateMap<ServiceClusterActiveHealthCheckConfig, ServiceClusterActiveHealthCheckConfigDto>()
            .ReverseMap();
        CreateMap<ServiceClusterPassiveHealthCheckConfig, ServiceClusterPassiveHealthCheckConfigDto>()
            .ReverseMap();

        CreateMap<ServiceRoute, GetServiceRouteRes>()
            .ForMember(dest => dest.MatchHosts,
                opt => opt.MapFrom(src => src.Match.Hosts.SplitAs(GatewayConstant.Separator, StringSplitOptions.RemoveEmptyEntries)))
            .ForMember(dest => dest.MatchMethods,
                opt => opt.MapFrom(src => src.Match.Methods.SplitAs(GatewayConstant.Separator, StringSplitOptions.RemoveEmptyEntries)))
            .ForMember(opt => opt.Transforms,
                dest => dest.MapFrom(opt => opt.Transforms.ToLookup(x => x.GroupIndex).ToDictionary(x => x.Key).ToList()));
        CreateMap<ServiceRouteTransform, ServiceRouteTransformDto>();
    }
}
