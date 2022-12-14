using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections;

namespace NetNet.Gateway.Swagger.SwaggerForYarp;

public class YarpSwaggerEndpointEnumerator : IEnumerable<UrlDescriptor>
{
    private readonly IConfiguration _configuration;
    private readonly IReverseProxyStore _proxyConfigStore;

    public YarpSwaggerEndpointEnumerator(IServiceProvider serviceProvider)
    {

        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _proxyConfigStore = serviceProvider.GetRequiredService<IReverseProxyStore>();
    }

    public IEnumerator<UrlDescriptor> GetEnumerator()
    {
        var proxyConfig = _proxyConfigStore.GetConfig();
        var enabledSwaggerClusters = proxyConfig.Clusters
            .Where(x => x.Metadata.GetValueOrDefault(YarpConstant.MetadataKeys.SwaggerEnabled, false));

        foreach (var cluster in enabledSwaggerClusters)
        {
            var name = cluster.Metadata.GetValueOrDefault(YarpConstant.MetadataKeys.SwaggerDocName, cluster.ClusterId);

            yield return new() { Name = name, Url = $"/swagger/docs/{cluster.ClusterId}" };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
