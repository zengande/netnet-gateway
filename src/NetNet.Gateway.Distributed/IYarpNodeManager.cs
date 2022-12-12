using NetNet.Gateway.Distributed.Configurations;
using NetNet.Gateway.Distributed.Models;

namespace NetNet.Gateway.Distributed;

public interface IYarpNodeManager
{
    Task RegisterAsync(string nodeId, YarpNodeType nodeType, TimeSpan heartbeat);
    Task UnRegisterAsync(string nodeId);
    Task HeartbeatAsync(string nodeId);
    IAsyncEnumerable<ServerNode> GetAllServerNodesAsync(CancellationToken cancellationToken = default);
}
