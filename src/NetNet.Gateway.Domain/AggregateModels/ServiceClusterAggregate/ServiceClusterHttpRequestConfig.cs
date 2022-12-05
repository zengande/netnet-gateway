using System.Net;
using Volo.Abp.Domain.Values;
using Yarp.ReverseProxy.Forwarder;

namespace NetNet.Gateway.AggregateModels.ServiceClusterAggregate;

public class ServiceClusterHttpRequestConfig : ValueObject
{
    /// <summary>
    /// 超时秒数 <br/>
    /// How long a request is allowed to remain idle between any operation completing, after which it will be canceled.
    /// The default is 100 seconds. The timeout will reset when response headers are received or after successfully reading or writing any request, response, or streaming data like gRPC or WebSockets.
    /// TCP keep-alives and HTTP/2 protocol pings will not reset the timeout, but WebSocket pings will.
    /// </summary>
    public long? ActivityTimeoutSeconds { get; set; }

    /// <summary>
    /// Allows to use write buffering when sending a response back to the client, if the server hosting YARP (e.g. IIS) supports it.
    /// NOTE: enabling it can break SSE (server side event) scenarios.
    /// </summary>
    public bool? AllowResponseBuffering { get; set; }

    /// <summary>
    /// Preferred version of the outgoing request. The default is HTTP/2.0.
    /// </summary>
    public Version? Version { get; set; } = HttpVersion.Version20;

    /// <summary>
    /// The policy applied to version selection, e.g. whether to prefer downgrades, upgrades or request an exact version. The default is RequestVersionOrLower.
    /// </summary>
    public HttpVersionPolicy? VersionPolicy { get; set; } = HttpVersionPolicy.RequestVersionOrLower;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ActivityTimeoutSeconds;
        yield return AllowResponseBuffering;
        yield return Version;
        yield return VersionPolicy;
    }

    public ForwarderRequestConfig ToYarpForwarderRequestConfig() => new()
    {
        ActivityTimeout = ActivityTimeoutSeconds.HasValue ? TimeSpan.FromSeconds(ActivityTimeoutSeconds.Value) : null,
        AllowResponseBuffering = AllowResponseBuffering,
        Version = Version,
        VersionPolicy = VersionPolicy
    };
}
