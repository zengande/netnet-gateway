using Microsoft.EntityFrameworkCore;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NetNet.Gateway.Repositories;

public class ServiceClusterRepository : EfCoreRepository<GatewayDbContext, ServiceCluster, Guid>, IServiceClusterRepository
{
    public ServiceClusterRepository(IDbContextProvider<GatewayDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<ServiceCluster> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = new
        CancellationToken())
    {
        var cluster = await (await GetDbSetAsync())
            .Include(x => x.Destinations)
            .Include(x => x.HealthCheckConfig)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (cluster == null)
        {
            throw new EntityNotFoundException(typeof(ServiceCluster), id);
        }

        return cluster;
    }
}
