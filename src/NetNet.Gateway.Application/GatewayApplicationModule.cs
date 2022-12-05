using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule),
    typeof(GatewayApplicationContractsModule),
    typeof(GatewayDomainModule)
)]
public class GatewayApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<GatewayAutoMapperProfile>();
        });
    }
}
