using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetNet.Gateway.Distributed.Configurations;
using Volo.Abp.Timing;

namespace NetNet.Gateway.Distributed.BackgroundTasks;

public class RemoveDeadReverseProxyServerNode : BackgroundService
{
    private readonly IYarpNodeManager _yarpNodeManager;
    private readonly IClock _clock;
    private readonly ILogger<RemoveDeadReverseProxyServerNode> _logger;
    private readonly YarpDistributedConfig _distributedConfig;

    public RemoveDeadReverseProxyServerNode(IYarpNodeManager yarpNodeManager, IClock clock, ILogger<RemoveDeadReverseProxyServerNode> logger,
        IOptions<YarpDistributedConfig> options)
    {
        _yarpNodeManager = yarpNodeManager;
        _clock = clock;
        _logger = logger;
        _distributedConfig = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = _clock.Now;
            await foreach (var node in _yarpNodeManager.GetAllServerNodesAsync(stoppingToken))
            {
                if (now - node.Heartbeat > _distributedConfig.HeartRate)
                {
                    _logger.LogInformation("Server node {0} dead!", node.NodeId);
                    await _yarpNodeManager.UnRegisterAsync(node.NodeId);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
        }
    }
}
