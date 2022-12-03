using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(GatewayApplicationContractsModule),
    typeof(GatewayDomainModule)
)]
public class GatewayApplicationModule : AbpModule
{
}