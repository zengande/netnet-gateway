using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RegisterReverseProxyConfigEventDispatcher : BackgroundService
{
    private const string ReverseProxyConfigChangedEventName = "ReverseProxyConfigChanged";

    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IReverseProxyStore _store;

    public RegisterReverseProxyConfigEventDispatcher(IConnectionMultiplexer connectionMultiplexer, IReverseProxyStore store)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriber = _connectionMultiplexer.GetSubscriber();

        // 配置发生变更时发送通知
        _store.OnConfigChanged -= _store.ReloadConfig;
        _store.OnConfigChanged += () => subscriber.Publish(ReverseProxyConfigChangedEventName, string.Empty);
    }
}

