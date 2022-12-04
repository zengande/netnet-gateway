using Volo.Abp.Domain.Entities;

namespace NetNet.Gateway.Entities;

public class CertificateConfig : Entity<long>
{
    public string Path { get; set; }

    public string KeyPath { get; set; }

    public string Password { get; set; }

    public string Subject { get; set; }

    public string Store { get; set; }

    public string Location { get; set; }

    public bool? AllowInvalid { get; set; }

    internal bool IsFileCert => !string.IsNullOrEmpty(Path);

    internal bool IsStoreCert => !string.IsNullOrEmpty(Subject);

    public int ProxyHttpClientOptionsId { get; set; }

    public virtual HttpClientConfig ProxyHttpClientOptions { get; set; }
}
