using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(GatewayDomainSharedModule)
)]
public class GatewayDomainModule : AbpModule
{
}