using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetNet.Gateway.Distributed.Configurations;
using NetNet.Gateway.Distributed.Models;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RegisterReverseProxyServerNode : BackgroundService
{
    private readonly IYarpNodeManager _nodeManager;
    private readonly CurrentNodeInfo _currentNodeConfig;
    private readonly YarpDistributedConfig _distributedConfig;

    public RegisterReverseProxyServerNode(IYarpNodeManager nodeManager, CurrentNodeInfo currentNodeConfig, IOptions<YarpDistributedConfig> options)
    {
        _nodeManager = nodeManager;
        _currentNodeConfig = currentNodeConfig;
        _distributedConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _nodeManager.RegisterAsync(_currentNodeConfig.NodeId, _currentNodeConfig.NodeType, _distributedConfig.HeartRate);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_distributedConfig.HeartRate, stoppingToken);

            // 心跳
            await _nodeManager.HeartbeatAsync(_currentNodeConfig.NodeId, _currentNodeConfig.NodeType, _distributedConfig.HeartRate);
        }

        await _nodeManager.UnRegisterAsync(_currentNodeConfig.NodeId);
    }
}
