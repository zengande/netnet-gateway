using Microsoft.Extensions.Primitives;
using Volo.Abp.Domain.Repositories;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

public interface IServiceClusterRepository : IRepository<ServiceCluster, long>
{
}
