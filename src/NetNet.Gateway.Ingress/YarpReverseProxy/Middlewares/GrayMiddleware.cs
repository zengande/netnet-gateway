namespace NetNet.Gateway.Ingress.YarpReverseProxy.Middlewares;

public class GrayMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
    }
}
