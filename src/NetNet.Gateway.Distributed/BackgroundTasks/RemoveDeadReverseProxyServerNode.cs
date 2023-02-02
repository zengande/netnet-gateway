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
        _distributedConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var csRedisClientLock = RedisHelper.Lock(nameof(RemoveDeadReverseProxyServerNode), 2);
            if (csRedisClientLock is null)
            {
                _logger.LogInformation("任务已被占用");
                continue;
            }

            var now = _clock.Now;
            await foreach (var node in _yarpNodeManager.GetAllServerNodesAsync(stoppingToken))
            {
                if (now - node.Heartbeat > _distributedConfig.HeartRate)
                {
                    _logger.LogInformation("Server node {0} dead!", node.NodeId);
                    await _yarpNodeManager.UnRegisterAsync(node.NodeId);
                }
            }

            await Task.Delay(_distributedConfig.HeartRate.Add(TimeSpan.FromSeconds(3)), stoppingToken);

            csRedisClientLock.Unlock();
        }
    }
}
