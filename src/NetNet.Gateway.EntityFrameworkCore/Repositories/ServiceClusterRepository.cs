using Microsoft.Extensions.Primitives;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NetNet.Gateway.Repositories;

public class ServiceClusterRepository : EfCoreRepository<GatewayDbContext, ServiceCluster, Guid>, IServiceClusterRepository
{
    public ServiceClusterRepository(IDbContextProvider<GatewayDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
