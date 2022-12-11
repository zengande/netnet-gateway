namespace NetNet.Gateway.Distributed.Configurations;

public class CurrentNodeConfig
{
    public string NodeId { get; set; } = Guid.NewGuid().ToString("N");

    public YarpNodeType NodeType { get; set; } = YarpNodeType.Unknown;
}

public enum YarpNodeType
{
    Unknown = 0,
    Admin = 1,
    Ingress,
}
