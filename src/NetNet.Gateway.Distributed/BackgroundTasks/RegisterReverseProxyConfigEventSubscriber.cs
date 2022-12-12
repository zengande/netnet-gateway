using Microsoft.Extensions.Hosting;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RegisterReverseProxyConfigEventSubscriber : BackgroundService
{
    private const string ReverseProxyConfigChangedEventName = "ReverseProxyConfigChanged";

    private readonly IReverseProxyStore _store;

    public RegisterReverseProxyConfigEventSubscriber(IReverseProxyStore store)
    {
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 订阅其他节点的配置变更事件
        RedisHelper.Subscribe((ReverseProxyConfigChangedEventName, _=> _store.RaiseConfigChanged()));
    }
}

