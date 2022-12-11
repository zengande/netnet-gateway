using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RegisterReverseProxyConfigEventSubscriber : BackgroundService
{
    private const string ReverseProxyConfigChangedEventName = "ReverseProxyConfigChanged";

    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IReverseProxyStore _store;

    public RegisterReverseProxyConfigEventSubscriber(IConnectionMultiplexer connectionMultiplexer, IReverseProxyStore store)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = _connectionMultiplexer.GetSubscriber();
        // 订阅其他节点的配置变更事件
        await subscriber.SubscribeAsync(ReverseProxyConfigChangedEventName,
            (_, _) => _store.RaiseConfigChanged());
    }
}

