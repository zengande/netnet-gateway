using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace NetNet.Gateway;

[DependsOn(
    typeof(GatewayDomainModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
)]
public class GatewayEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GatewayDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also BMSMigrationsDbContextFactory for EF Core tooling. */
            options.Configure<GatewayDbContext>(opts =>
            {
                // opts.UseNpgsql();
                opts.UseSqlServer();
#if DEBUG
                opts.DbContextOptions.EnableSensitiveDataLogging();
#endif
            });
        });
    }
}
