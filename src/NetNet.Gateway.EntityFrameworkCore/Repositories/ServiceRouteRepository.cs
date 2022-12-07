using Microsoft.EntityFrameworkCore;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NetNet.Gateway.Repositories;

public class ServiceRouteRepository : EfCoreRepository<GatewayDbContext, ServiceRoute, Guid>, IServiceRouteRepository
{
    public ServiceRouteRepository(IDbContextProvider<GatewayDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<ServiceRoute>> WithDetailsAsync()
    {
        return (await GetDbSetAsync())
            .Include(x => x.Match);
    }
}
