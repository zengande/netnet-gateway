using NetNet.Gateway.Admin.Data;
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
        context.Services.AddRazorPages();
        context.Services.AddServerSideBlazor();
        context.Services.AddSingleton<WeatherForecastService>();
        context.Services.AddBootstrapBlazor();
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
