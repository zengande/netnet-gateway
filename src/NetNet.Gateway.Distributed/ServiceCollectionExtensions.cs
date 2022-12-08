using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace NetNet.Gateway;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYarpDistributedRedis(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

        return services;
    }

    /// <summary>
    /// 管理节点分发配置修改通知
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddYarpRedisDistributedEventDispatcher(this IServiceCollection services)
    {
        services.AddHostedService<RegisterReverseProxyConfigEventDispatcher>();

        return services;
    }
}
