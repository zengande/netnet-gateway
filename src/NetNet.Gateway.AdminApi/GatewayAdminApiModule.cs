using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetNet.Gateway.Distributed;
using NetNet.Gateway.Distributed.Extensions;
using NetNet.Gateway.Distributed.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace NetNet.Gateway.AdminApi;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(GatewayDistributedModule),
    typeof(GatewayEntityFrameworkCoreModule),
    typeof(GatewayApplicationModule)
)]
public class GatewayAdminApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAspNetCoreMvcOptions>(opt =>
        {
            opt.ConventionalControllers.Create(typeof(GatewayApplicationModule).Assembly);
        });

        context.Services
            .AddYarpDistributedRedis(config =>
            {
                config.RedisConnectionString = configuration.GetValue<string>("Redis:Configuration")!;
            })
            .AddYarpRedisDistributedEventDispatcher()
            .AddServerNode(YarpNodeType.Admin);

        context.Services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "NetNet Gateway AdminAPI", Version = "v1" });
                options.DocInclusionPredicate((doc, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        context.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
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
                    OnAccessDenied = ctx =>
                    {
                        ctx.HandleResponse();
                        ctx.Response.Redirect("/");
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

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "NetNet Gateway AdminAPI";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "NetNet Gateway Admin");
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseConfiguredEndpoints();
    }
}
