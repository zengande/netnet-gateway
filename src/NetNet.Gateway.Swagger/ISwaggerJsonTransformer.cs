using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Swagger;

/// <summary>
/// swagger json 转换器
/// </summary>
public interface ISwaggerJsonTransformer
{
    /// <summary>
    /// Transforms downstream swagger json into upstream format.
    /// </summary>
    string Transform(string swaggerJson, ClusterConfig cluster);
}
