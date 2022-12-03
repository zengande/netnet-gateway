using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(GatewayDomainSharedModule)
)]
public class GatewayApplicationContractsModule : AbpModule
{
}