using Volo.Abp.DependencyInjection;

namespace NetNet.Gateway.Ingress.YarpReverseProxy.Middlewares;

public class GrayMiddleware : IMiddleware, ITransientDependency
{
    private readonly ILogger<GrayMiddleware> _logger;

    public GrayMiddleware(ILogger<GrayMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("灰度待实现");

        await next(context);

        context.Response.Headers.TryAdd("x-gray-version", "lalalala");
    }
}
