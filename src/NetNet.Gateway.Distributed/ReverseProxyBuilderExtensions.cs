using Microsoft.Extensions.DependencyInjection;

namespace NetNet.Gateway;

public static class ReverseProxyBuilderExtensions
{
    /// <summary>
    /// 当前节点做为入口节点时
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IReverseProxyBuilder AddRedisEventSubscriber(this IReverseProxyBuilder builder)
    {
        builder.Services.AddHostedService<RegisterReverseProxyConfigEventSubscriber>();

        return builder;
    }
}
