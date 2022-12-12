using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace NetNet.Gateway.Distributed;

[DependsOn(typeof(AbpTimingModule))]
public class GatewayDistributedModule : AbpModule
{
}
