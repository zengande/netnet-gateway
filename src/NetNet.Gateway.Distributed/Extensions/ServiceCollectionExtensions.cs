using Microsoft.Extensions.DependencyInjection;
using NetNet.Gateway.Distributed.BackgroundTasks;
using NetNet.Gateway.Distributed.Configurations;
using NetNet.Gateway.Distributed.Models;

namespace NetNet.Gateway.Distributed.Extensions;

public static class ServiceCollectionExtensions
{
    public static IYarpDistributedBuilder AddYarpDistributedRedis(this IServiceCollection services, Action<YarpDistributedConfig> action)
    {
        services.Configure(action);

        var config = new YarpDistributedConfig();
        action(config);
        RedisHelper.Initialization(new(config.RedisConnectionString));

        return new YarpDistributedBuilder(services);
    }

    /// <summary>
    /// 分发配置修改通知(管理节点)
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder AddYarpRedisDistributedEventDispatcher(this IYarpDistributedBuilder builder)
    {
        builder.Services.AddHostedService<RegisterReverseProxyConfigEventDispatcher>();
        builder.Services.AddHostedService<RemoveDeadReverseProxyServerNode>();

        return builder;
    }

    /// <summary>
    /// 接收配置变更通知
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
    public static IYarpDistributedBuilder AddServerNode(this IYarpDistributedBuilder builder,
        YarpNodeType nodeType)
    {
        return AddServerNode(builder, config => config.NodeType = nodeType);
    }

    /// <summary>
    /// 配置当前节点
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IYarpDistributedBuilder AddServerNode(this IYarpDistributedBuilder builder,
        Action<CurrentNodeInfo> action)
    {
        var config = new CurrentNodeInfo();
        action(config);

        builder.Services.AddSingleton(config);
        builder.Services.AddHostedService<RegisterReverseProxyServerNode>();

        return builder;
    }
}
