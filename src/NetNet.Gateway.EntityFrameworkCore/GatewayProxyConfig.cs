using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

[Serializable]
public class GatewayProxyConfig : IProxyConfig
{
    [JsonIgnore] public IChangeToken? ChangeToken { get; internal set; }

    IReadOnlyList<RouteConfig> IProxyConfig.Routes => Routes;
    IReadOnlyList<ClusterConfig> IProxyConfig.Clusters => Clusters;

    public List<RouteConfig> Routes { get; internal set; }

    public List<ClusterConfig> Clusters {get; internal set; }

    public GatewayProxyConfig(List<RouteConfig> routes, List<ClusterConfig> clusters)
    {
        Routes = routes;
        Clusters = clusters;
    }
}
