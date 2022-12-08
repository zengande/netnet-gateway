using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace NetNet.Gateway;

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

        // // 当当前节点配置发生变更时发送通知
        // _store.OnConfigChanged -= _store.ReloadConfig;
        // _store.OnConfigChanged += () => subscriber.Publish(ReverseProxyConfigChangedEventName, string.Empty);

        // 订阅其他节点的配置变更事件
        await subscriber.SubscribeAsync(ReverseProxyConfigChangedEventName,
            (_, _) => _store.RaiseConfigChanged());
    }
}

