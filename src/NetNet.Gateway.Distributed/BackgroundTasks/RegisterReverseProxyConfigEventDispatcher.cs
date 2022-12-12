using Microsoft.Extensions.Hosting;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RegisterReverseProxyConfigEventDispatcher : BackgroundService
{
    private const string ReverseProxyConfigChangedEventName = "ReverseProxyConfigChanged";

    private readonly IReverseProxyStore _store;

    public RegisterReverseProxyConfigEventDispatcher(IReverseProxyStore store)
    {
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 配置发生变更时发送通知
        _store.OnConfigChanged -= _store.ReloadConfig;
        _store.OnConfigChanged += () => RedisHelper.Publish(ReverseProxyConfigChangedEventName, string.Empty);
    }
}
