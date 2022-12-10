using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

[Serializable]
public class GatewayProxyConfig : IProxyConfig
{
    [JsonIgnore] public IChangeToken? ChangeToken { get; private set; }

    IReadOnlyList<RouteConfig> IProxyConfig.Routes => Routes;
    IReadOnlyList<ClusterConfig> IProxyConfig.Clusters => Clusters;

    public string Version { get; private set; }

    public List<RouteConfig> Routes { get; private set; }

    public List<ClusterConfig> Clusters {get; private set; }

    public GatewayProxyConfig(string version, List<RouteConfig> routes, List<ClusterConfig> clusters)
    {
        Version = version;
        Routes = routes;
        Clusters = clusters;
    }

    public void SetChangeToken(IChangeToken changeToken)
    {
        ChangeToken = changeToken;
    }
}
