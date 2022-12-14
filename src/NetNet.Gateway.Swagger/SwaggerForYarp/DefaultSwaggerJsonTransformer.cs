using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.Swagger.SwaggerForYarp;

public class DefaultSwaggerJsonTransformer : ISwaggerJsonTransformer, IScopedDependency
{
    public string Transform(string swaggerJson, ClusterConfig cluster)
    {
        // SwaggerJsonTransformBuilder.Build ==(SwaggerJsonTransformBuilderContext)=> SwaggerJsonTransformer =>
        return swaggerJson;
    }
}
