using Microsoft.Extensions.Logging;
using NetNet.Gateway.Distributed.Models;
using System.Runtime.CompilerServices;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace NetNet.Gateway.Distributed;

public class RedisYarpNodeManager : IYarpNodeManager, IScopedDependency
{
    private readonly ILogger<RedisYarpNodeManager> _logger;
    private readonly IClock _clock;

    public RedisYarpNodeManager(IClock clock, ILogger<RedisYarpNodeManager> logger)
    {
        _clock = clock;
        _logger = logger;
    }

    public async Task RegisterAsync(string nodeId, YarpNodeType nodeType, TimeSpan heartbeat)
    {
        Check.NotNullOrWhiteSpace(nodeId, nameof(nodeId));

        var key = CreateHashKey(nodeId);
        var now = _clock.Now;
        await RedisHelper.HMSetAsync(key, "NodeId", nodeId, "NodeType", nodeType.ToString(), "StartedAt", now, "Heartbeat", now.Add(heartbeat));
    }

    public async Task UnRegisterAsync(string nodeId)
    {
        var key = CreateHashKey(nodeId);
        await RedisHelper.DelAsync(key);
    }

    public async Task HeartbeatAsync(string nodeId, YarpNodeType nodeType, TimeSpan heartbeat)
    {
        var key = CreateHashKey(nodeId);
        if (!await RedisHelper.ExistsAsync(key))
        {
            _logger.LogWarning($"节点{nodeId}不存在");

            await RegisterAsync(nodeId, nodeType, heartbeat);
            return;
        }

        var now = _clock.Now;
        _logger.LogDebug("Node {0} heartbeat", key);

        await RedisHelper.HSetAsync(key, "Heartbeat", now);
    }

    public async IAsyncEnumerable<ServerNode> GetAllServerNodesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var nodes = await RedisHelper.KeysAsync("netnet:servernodes:*");
        foreach (var node in nodes)
        {
            var dictionary = await RedisHelper.HGetAllAsync(node);

            Enum.TryParse<YarpNodeType>(dictionary.GetValueOrDefault("NodeType", string.Empty), out var nodeType);
            DateTime.TryParse(dictionary.GetValueOrDefault("StartedAt", string.Empty), out var startedAt);
            DateTime.TryParse(dictionary.GetValueOrDefault("Heartbeat", string.Empty), out var heartbeat);
            var nodeId = dictionary.GetValueOrDefault("NodeId", string.Empty);
            if (string.IsNullOrWhiteSpace(nodeId)) continue;

            yield return new ServerNode
            {
                NodeId = nodeId, NodeType = nodeType, StartedAt = startedAt, Heartbeat = heartbeat,
            };
        }
    }

    private string CreateHashKey(string nodeId) => $"netnet:servernodes:{nodeId}";
}
