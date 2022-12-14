using System.Reflection;

namespace NetNet.Gateway.BuildingBlock.Configurations;

public class GatewayRouterOptions
{
    public Assembly AppAssembly { get; set; }

    public List<Assembly> AdditionalAssemblies { get; }

    public GatewayRouterOptions()
    {
        AdditionalAssemblies = new();
    }
}
