using NetNet.Gateway.Distributed.Configurations;

namespace NetNet.Gateway.Distributed.Models;

public class ServerNode
{
    /// <summary>
    /// 节点id
    /// </summary>
    public string NodeId { get; set; }

    /// <summary>
    /// 节点类型
    /// </summary>
    public YarpNodeType NodeType { get; set; }

    /// <summary>
    /// 启动时间
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 心跳时间
    /// </summary>
    public DateTime Heartbeat { get; set; }
}
