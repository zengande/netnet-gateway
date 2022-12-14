using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Logging;
using System.Text;
using Volo.Abp.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Swagger.SwaggerForYarp;

/// <summary>
/// 请求 swagger json 时，下载下游服务的json，经过转换后返回
/// </summary>
public class SwaggerForYarpMiddleware : IMiddleware, IScopedDependency
{
    private readonly TemplateMatcher _requestMatcher;
    private readonly ILogger<SwaggerForYarpMiddleware> _logger;
    private readonly IDownstreamSwaggerDocsDownloader _downstreamSwaggerDocsDownloader;
    private readonly ISwaggerJsonTransformer _transformer;
    private readonly IReverseProxyStore _reverseProxyStore;

    public SwaggerForYarpMiddleware(ILogger<SwaggerForYarpMiddleware> logger, IDownstreamSwaggerDocsDownloader downstreamSwaggerDocsDownloader,
        ISwaggerJsonTransformer transformer, IReverseProxyStore reverseProxyStore)
    {
        _logger = logger;
        _downstreamSwaggerDocsDownloader = downstreamSwaggerDocsDownloader;
        _transformer = transformer;
        _reverseProxyStore = reverseProxyStore;
        _requestMatcher = new(TemplateParser.Parse("/{clusterId}"), new());
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // eg: bms
        if (RequestingClusterInfo(context.Request, out var cluster))
        {
            _logger.LogInformation("Matched swagger endpoint clusterId: {0}", cluster!.ClusterId);

            // 请求 swagger key 对应的 swagger.json 地址
            var content = await _downstreamSwaggerDocsDownloader.GetSwaggerJsonAsync(cluster);

            if (!string.IsNullOrWhiteSpace(content))
            {
                


                // 将 swagger 配置重写
                content = _transformer.Transform(content, cluster);
            }

            // 响应
            await context.Response.WriteAsync(content, Encoding.UTF8);
            return;
        }

        await next(context);
    }

    private bool RequestingClusterInfo(HttpRequest request, out ClusterConfig? clusterInfo)
    {
        clusterInfo = null;
        if (request.Method != "GET") return false;

        var routeValues = new RouteValueDictionary();
        if (!_requestMatcher.TryMatch(request.Path, routeValues) ||
            !routeValues.ContainsKey("clusterId")) return false;

        var clusterId = routeValues["clusterId"]!.ToString();
        clusterInfo = _reverseProxyStore.GetConfig()
            .Clusters
            .SingleOrDefault(x => x.ClusterId.Equals(clusterId, StringComparison.OrdinalIgnoreCase));

        return true;
    }
}
