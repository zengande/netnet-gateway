using NetNet.Gateway.Distributed.Configurations;

namespace NetNet.Gateway.Distributed.Interfaces;

public interface IYarpNodeManager
{
    Task RegisterAsync(string nodeId, YarpNodeType nodeType);

    Task UnRegisterAsync(string nodeId);
}
