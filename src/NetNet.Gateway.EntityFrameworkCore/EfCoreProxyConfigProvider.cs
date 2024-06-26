using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public class EfCoreProxyConfigProvider : IProxyConfigProvider, IDisposable
{
    private static readonly object LockObject = new();
    private IProxyConfig? _config;
    private CancellationTokenSource _changeToken;
    private bool _disposed;
    private readonly IDisposable _subscription;
    private readonly IReverseProxyStore _store;

    public EfCoreProxyConfigProvider(IReverseProxyStore store)
    {
        _disposed = false;
        _store = store;

        _subscription = ChangeToken.OnChange(_store.GetReloadToken, () => LoadConfig(false));
    }

    public IProxyConfig GetConfig()
    {
        if (_config is null)
        {
            LoadConfig(true);
        }

        return _config!;
    }

    private void LoadConfig(bool forceReload = false)
    {
        // Prevent overlapping updates, especially on startup.
        lock (LockObject)
        {
            GatewayProxyConfig? config;
            try
            {
                config = (_store.GetConfig(forceReload) as GatewayProxyConfig)!;
            }
            catch (Exception ex)
            {
                // Re-throw on the first time load to prevent app from starting.
                if (_config == null)
                {
                    throw;
                }

                return;
            }

            var oldToken = _changeToken;
            _changeToken = new CancellationTokenSource();
            config.SetChangeToken(new CancellationChangeToken(_changeToken.Token));
            _config = config;

            try
            {
                oldToken?.Cancel(throwOnFirstException: false);
            }
            catch (Exception ex)
            {
            }
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _subscription.Dispose();
            _changeToken.Dispose();
            _disposed = true;
        }
    }
}
