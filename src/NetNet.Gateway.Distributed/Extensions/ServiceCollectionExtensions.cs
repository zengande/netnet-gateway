using Microsoft.Extensions.DependencyInjection;
using NetNet.Gateway.Distributed.BackgroundTasks;
using NetNet.Gateway.Distributed.Configurations;
using NetNet.Gateway.Distributed.Interfaces;
using StackExchange.Redis;

namespace NetNet.Gateway.Distributed.Extensions;

public static class ServiceCollectionExtensions
{
    public static IYarpDistributedBuilder AddYarpDistributedRedis(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

        return new YarpDistributedBuilder(services);
    }

    /// <summary>
    /// 管理节点分发配置修改通知
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder AddYarpRedisDistributedEventDispatcher(this IYarpDistributedBuilder builder)
    {
        builder.Services.AddHostedService<RegisterReverseProxyConfigEventDispatcher>();

        return builder;
    }

    /// <summary>
    /// 当前节点做为入口节点时接收配置变更通知
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder AddRedisEventSubscriber(this IYarpDistributedBuilder builder)
    {
        builder.Services.AddHostedService<RegisterReverseProxyConfigEventSubscriber>();

        return builder;
    }

    /// <summary>
    /// 配置当前节点
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="nodeType"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder ConfigureCurrentNode(this IYarpDistributedBuilder builder,
        YarpNodeType nodeType)
    {
        return ConfigureCurrentNode(builder, config => config.NodeType = nodeType);
    }

    /// <summary>
    /// 配置当前节点
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder ConfigureCurrentNode(this IYarpDistributedBuilder builder,
        Action<CurrentNodeConfig> action)
    {
        var config = new CurrentNodeConfig();
        action(config);

        builder.Services.AddSingleton(config);

        return builder;
    }
}
