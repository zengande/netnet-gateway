using Microsoft.Extensions.Logging;
using NetNet.Gateway.Events.ServiceClusters;
using NetNet.Gateway.Events.ServiceRoutes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace NetNet.Gateway.DomainEventHandlers;

public class ServiceClusterOrRouteChangedDomainEventHandler
    : ILocalEventHandler<ServiceClusterChangedDomainEvent>, ILocalEventHandler<ServiceRouteChangedDomainEvent>, ITransientDependency
{
    private readonly ILogger<ServiceClusterOrRouteChangedDomainEventHandler> _logger;
    private readonly IReverseProxyStore _store;

    public ServiceClusterOrRouteChangedDomainEventHandler(ILogger<ServiceClusterOrRouteChangedDomainEventHandler> logger, IReverseProxyStore store)
    {
        _logger = logger;
        _store = store;
    }

    public Task HandleEventAsync(ServiceClusterChangedDomainEvent eventData)
    {
        return RaiseConfigChanged();
    }

    public Task HandleEventAsync(ServiceRouteChangedDomainEvent eventData)
    {
        return RaiseConfigChanged();
    }

    private Task RaiseConfigChanged()
    {
        _logger.LogInformation("通知 YARP 配置发生改变");

        return Task.Run(_store.RaiseConfigChanged);
    }
}
