using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NetNet.Gateway.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerForYarpGen(this IServiceCollection services, Action<SwaggerGenOptions>? setupAction = null)
    {
        services.AddSwaggerGen(options =>
        {
            setupAction?.Invoke(options);
        });

        return services;
    }
}
