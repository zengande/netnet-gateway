using Microsoft.Extensions.Primitives;

namespace NetNet.Gateway;

public class ReverseProxyConfigChangeToken : IChangeToken
{
    private readonly CancellationTokenSource _tokenSource = new();

    public IDisposable RegisterChangeCallback(Action<object> callback, object state)
    {
        return _tokenSource.Token.Register(callback!, state);
    }

    public bool ActiveChangeCallbacks => true;
    public bool HasChanged => _tokenSource.IsCancellationRequested;

    public void OnReload() => _tokenSource.Cancel();
}
