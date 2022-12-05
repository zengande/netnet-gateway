using Volo.Abp.Application.Services;

namespace NetNet.Gateway;

public abstract class GatewayAppService : ApplicationService
{
    protected IQueryableWrapperFactory QueryableWrapperFactory => LazyServiceProvider.LazyGetRequiredService<IQueryableWrapperFactory>();
}
