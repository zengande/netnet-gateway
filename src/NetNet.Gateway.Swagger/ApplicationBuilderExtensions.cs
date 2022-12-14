using Microsoft.AspNetCore.Builder;
using NetNet.Gateway.Swagger.SwaggerForYarp;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NetNet.Gateway.Swagger;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerUIWithYarp(this IApplicationBuilder app, Action<SwaggerUIOptions>? setupAction = null)
    {
        var serviceProvider = app.ApplicationServices;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "/{documentName}/swagger.{json|yaml}";
        });
        app.UseSwaggerForYarp();
        app.UseSwaggerUI(options =>
        {
            // 每次刷新 swagger 的时候都会重新获取
            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1093#issuecomment-964622538
            options.ConfigObject.Urls = new YarpSwaggerEndpointEnumerator(serviceProvider);

            setupAction?.Invoke(options);
        });

        return app;
    }

    public static IApplicationBuilder UseSwaggerForYarp(this IApplicationBuilder app)
    {
        app.Map("/swagger/docs", builder => builder.UseMiddleware<SwaggerForYarpMiddleware>());

        return app;
    }

    public static T GetValueOrDefault<T>(this IReadOnlyDictionary<string, string>? mateData, string key, T defaultValue)
    {
        if (mateData?.TryGetValue(key, out var value) == true)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        return defaultValue;
    }
}
