using Microsoft.Extensions.DependencyInjection;

namespace NetNet.Gateway.Distributed;

public interface IYarpDistributedBuilder
{
    IServiceCollection Services { get; }
}

public class YarpDistributedBuilder : IYarpDistributedBuilder
{
    public YarpDistributedBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
}
