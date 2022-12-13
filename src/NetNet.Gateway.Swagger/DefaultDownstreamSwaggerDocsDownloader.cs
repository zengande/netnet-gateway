using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Swagger;

public class DefaultDownstreamSwaggerDocsDownloader : IDownstreamSwaggerDocsDownloader, IScopedDependency
{
    private readonly ILogger<DefaultDownstreamSwaggerDocsDownloader> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultDownstreamSwaggerDocsDownloader(ILogger<DefaultDownstreamSwaggerDocsDownloader> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetSwaggerJsonAsync(ClusterConfig cluster)
    {
        var url = cluster.Metadata.GetValueOrDefault(YarpConstant.MetadataKeys.SwaggerJsonUrl, string.Empty);
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("Cluster {0} 未定义 SwaggerJsonUrl", cluster);
            return string.Empty;
        }

        var httpClient = _httpClientFactory.CreateClient();
        return await httpClient.GetStringAsync(url);
    }
}
