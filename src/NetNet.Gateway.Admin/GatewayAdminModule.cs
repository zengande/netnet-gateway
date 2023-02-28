using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetNet.Gateway.Admin.Configurations;
using NetNet.Gateway.BuildingBlock.Configurations;
using NetNet.Gateway.Distributed;
using NetNet.Gateway.Distributed.Extensions;
using NetNet.Gateway.Distributed.Models;
using NetNet.Gateway.Swagger;
using NetNet.Gateway.SwaggerUI.Blazor;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace NetNet.Gateway.Admin;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(GatewaySwaggerModule),
    typeof(GatewaySwaggerUIBlazorModule),
    typeof(GatewayDistributedModule),
    typeof(GatewayEntityFrameworkCoreModule),
    typeof(GatewayApplicationModule)
)]
public class GatewayAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<GatewayRouterOptions>(options =>
        {
            options.AppAssembly = typeof(App).Assembly;
        });

        var configuration = context.Services.GetConfiguration();

        context.Services.AddRazorPages();
        context.Services.AddServerSideBlazor();
        context.Services.AddBootstrapBlazor();

        context.Services
            .AddYarpDistributedRedis(config =>
            {
                config.RedisConnectionString = configuration.GetValue<string>("Redis:Configuration");
            })
            .AddYarpRedisDistributedEventDispatcher()
            .AddServerNode(YarpNodeType.Admin);

        Configure<GatewayAdminConfig>(configuration.GetSection("Gateway:Admin"));

        context.Services
            .AddSwaggerForYarpGen(options =>
            {
                options.SwaggerDoc("gateway", new OpenApiInfo { Title = "YaYa External Gateway", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        context.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                options.Authority = configuration.GetValue<string>("Oidc:Authority");
                options.ClientId = configuration.GetValue<string>("Oidc:ClientId");
                options.ClientSecret = configuration.GetValue<string>("Oidc:ClientSecret");
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = false;
                options.UseTokenLifetime = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" };

                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseStaticFiles();

        app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });

        // 添加内部服务的Swagger终点
        app.UseSwaggerUIWithYarp(options =>
        {
            options.DocumentTitle = "NetNet Gateway Admin";

            // 网关自己的 swagger
            options.SwaggerEndpoint("/gateway/swagger.json", "NetNet Gateway Admin");
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(route =>
        {
            route.MapBlazorHub();
            route.MapFallbackToPage("/_Host");
        });
    }
}
