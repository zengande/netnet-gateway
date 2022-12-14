using NetNet.Gateway.BuildingBlock.Configurations;
using Volo.Abp.Modularity;

namespace NetNet.Gateway.SwaggerUI.Blazor;

public class GatewaySwaggerUIBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<GatewayRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(GatewaySwaggerUIBlazorModule).Assembly);
        });
    }
}
