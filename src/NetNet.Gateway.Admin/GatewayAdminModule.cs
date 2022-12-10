﻿using NetNet.Gateway.Admin.Configurations;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NetNet.Gateway.Admin;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(GatewayEntityFrameworkCoreModule),
    typeof(GatewayApplicationModule)
)]
public class GatewayAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddRazorPages();
        context.Services.AddServerSideBlazor();
        context.Services.AddBootstrapBlazor();

        context.Services
            .AddYarpDistributedRedis(configuration.GetValue<string>("Redis:Configuration"))
            .AddYarpRedisDistributedEventDispatcher();

        Configure<GatewayAdminConfig>(configuration.GetSection("Gateway:Admin"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(route =>
        {
            route.MapBlazorHub();
            route.MapFallbackToPage("/_Host");
        });
    }
}
