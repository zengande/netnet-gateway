using NetNet.Gateway.Distributed;
using NetNet.Gateway.Distributed.Configurations;
using NetNet.Gateway.Distributed.Extensions;
using NetNet.Gateway.Ingress.YarpReverseProxy.Middlewares;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace NetNet.Gateway.Ingress;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(GatewayDistributedModule),
    typeof(GatewayEntityFrameworkCoreModule),
    typeof(GatewayApplicationModule)
)]
public class GatewayIngressModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services
            .AddYarpDistributedRedis(configuration.GetValue<string>("Redis:Configuration"))
            .AddRedisEventSubscriber()
            .ConfigureCurrentNode(YarpNodeType.Ingress);

        context.Services.AddReverseProxy()
            .LoadFromEfCore();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        // Enable endpoint routing, required for the reverse proxy
        app.UseRouting();

        // Register the reverse proxy routes
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapReverseProxy(proxyPipeline =>
            {
                proxyPipeline.UseMiddleware<GrayMiddleware>();
                proxyPipeline.UseSessionAffinity();
                proxyPipeline.UseLoadBalancing();
                proxyPipeline.UsePassiveHealthChecks();
            });
        });
    }
}
