using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public class EfCoreProxyConfigProvider : IProxyConfigProvider
{
    private readonly IServiceClusterRepository _serviceClusterRepository;

    public EfCoreProxyConfigProvider(IServiceClusterRepository serviceClusterRepository)
    {
        _serviceClusterRepository = serviceClusterRepository;
    }

    public IProxyConfig GetConfig()
    {


        return new GatewayProxyConfig(new(), new(), default!);
    }
}
