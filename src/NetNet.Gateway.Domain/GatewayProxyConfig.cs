using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public class GatewayProxyConfig : IProxyConfig
{
    public IReadOnlyList<RouteConfig> Routes => _routes;
    public IReadOnlyList<ClusterConfig> Clusters => _clusters;
    public IChangeToken ChangeToken { get; private set; }

    private readonly List<RouteConfig> _routes;
    private readonly List<ClusterConfig> _clusters;

    public GatewayProxyConfig(List<RouteConfig> routes, List<ClusterConfig> clusters, IChangeToken changeToken)
    {
        _routes = routes;
        _clusters = clusters;
        ChangeToken = changeToken;
    }
}
