using Volo.Abp.Domain.Repositories;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

public interface IServiceRouteRepository : IRepository<ServiceRoute, Guid>
{

}
