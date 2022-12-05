using System.Security.Authentication;
using Volo.Abp.Domain.Values;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

public class ServiceClusterHttpClientConfig : ValueObject
{
    /// <summary>
    /// Indicates if destination server https certificate errors should be ignored.
    /// This should only be done when using self-signed certificates.
    /// </summary>
    public bool? DangerousAcceptAnyServerCertificate { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether additional HTTP/2 connections can be established to the same server
    /// when the maximum number of concurrent streams is reached on all existing connections.
    /// </summary>
    public bool? EnableMultipleHttp2Connections { get; set; }

    /// <summary>
    /// Limits the number of connections used when communicating with the destination server.
    /// </summary>
    public int? MaxConnectionsPerServer { get; set; }

    /// <summary>
    /// Enables non-ASCII header encoding for outgoing requests.
    /// </summary>
    public string? RequestHeaderEncoding { get; set; }

    /// <summary>
    /// What TLS protocols to use.
    /// </summary>
    public SslProtocols? SslProtocols { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return DangerousAcceptAnyServerCertificate;
        yield return EnableMultipleHttp2Connections;
        yield return MaxConnectionsPerServer;
        yield return RequestHeaderEncoding;
        yield return SslProtocols;
    }

    public HttpClientConfig ToYarpHttpClientConfig() => new()
    {
        DangerousAcceptAnyServerCertificate = DangerousAcceptAnyServerCertificate,
        EnableMultipleHttp2Connections = EnableMultipleHttp2Connections,
        MaxConnectionsPerServer = MaxConnectionsPerServer,
        RequestHeaderEncoding = RequestHeaderEncoding,
        SslProtocols = SslProtocols
    };
}
