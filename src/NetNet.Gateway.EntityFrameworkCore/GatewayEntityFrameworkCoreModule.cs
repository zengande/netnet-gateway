using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(GatewayDomainModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class GatewayEntityFrameworkCoreModule : AbpModule
{
}