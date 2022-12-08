using Volo.Abp.Domain.Entities;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

public class ServiceRouteMatch : Entity<Guid>
{
    public Guid ServiceRouteId { get; private set; }

    /// <summary>
    /// 请求主机 eg: localhost:5001,10.0.1.2:5002
    /// </summary>
    public string? Hosts { get; private set; }

    /// <summary>
    /// 请求方法 eg: GET,POST,PUT
    /// </summary>
    public string? Methods { get; private set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string? Path { get; private set; }

    private ServiceRouteMatch() { }

    public ServiceRouteMatch(string? hosts, string? methods, string? path)
        : this()
    {
        Hosts = hosts;
        Methods = methods;
        Path = path;
    }

    public void Update(string? hosts, string? methods, string? path)
    {
        Hosts = hosts;
        Methods = methods;
        Path = path;
    }
}
