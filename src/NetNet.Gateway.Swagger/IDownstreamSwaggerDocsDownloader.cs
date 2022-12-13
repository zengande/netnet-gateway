using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Swagger;

/// <summary>
/// 下游swagger文档下载器
/// </summary>
public interface IDownstreamSwaggerDocsDownloader
{
    /// <summary>
    /// Gets the swagger json.
    /// </summary>
    Task<string> GetSwaggerJsonAsync(ClusterConfig cluster);
}
