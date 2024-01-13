using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(typeof(AbpDddDomainSharedModule))]
public class GatewayDomainSharedModule : AbpModule
{
}
