namespace NetNet.Gateway.Distributed.Models;

public class CurrentNodeInfo
{
    public string NodeId { get; set; } = Guid.NewGuid().ToString("N");

    public YarpNodeType NodeType { get; set; } = YarpNodeType.Unknown;
}
