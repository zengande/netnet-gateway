using Volo.Abp.Domain.Entities;

namespace NetNet.Gateway.AggregateModels.ServiceRouteAggregate;

public class ServiceRouteMatch : Entity<Guid>
{
    public Guid ServiceRouteId { get; private set; }
    public List<string>? Hosts { get; private set; }
    public List<string>? Methods { get; private set; }
    public string? Path { get; private set; }
    private ServiceRouteMatch() { }

    public ServiceRouteMatch(Guid serviceRouteId, List<string>? hosts, List<string>? methods, string? path)
        : this()
    {
        ServiceRouteId = serviceRouteId;
        Hosts = hosts;
        Methods = methods;
        Path = path;
    }
}
