using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public static class ReverseProxyBuilderExtensions
{
    public static IReverseProxyBuilder LoadFromEfCore(this IReverseProxyBuilder builder)
    {
        builder.Services.AddSingleton<IProxyConfigProvider, EfCoreProxyConfigProvider>();
        return builder;
    }
}
