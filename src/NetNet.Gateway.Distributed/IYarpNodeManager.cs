using NetNet.Gateway.Distributed.Models;

namespace NetNet.Gateway.Distributed;

public interface IYarpNodeManager
{
    Task RegisterAsync(string nodeId, YarpNodeType nodeType, TimeSpan heartbeat);
    Task UnRegisterAsync(string nodeId);
    Task HeartbeatAsync(string nodeId, YarpNodeType nodeType, TimeSpan heartbeat);
    IAsyncEnumerable<ServerNode> GetAllServerNodesAsync(CancellationToken cancellationToken = default);
}
