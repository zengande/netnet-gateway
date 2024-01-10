using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using NetNet.Gateway.Extensions;
using System.Text.Json;
using Volo.Abp.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

namespace NetNet.Gateway;

public class EfReverseProxyStore : IReverseProxyStore, ISingletonDependency
{
    private const string CacheKey = "netnet:gateway:proxyconfig";

    public event ConfigChangedHandler OnConfigChanged;

    private ReverseProxyConfigChangeToken _changeToken = new();

    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<EfReverseProxyStore> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EfReverseProxyStore(IDistributedCache distributedCache, ILogger<EfReverseProxyStore> logger, IServiceProvider serviceProvider)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _serviceProvider = serviceProvider;

        OnConfigChanged += ReloadConfig;
    }

    public IProxyConfig GetConfig(bool forceReload = false)
    {
        GatewayProxyConfig? config = forceReload ? null : TryGetFromCache();
        if (config is null)
        {
            config = GetFromDataBase();
            SetToCache(config);
        }

        return config;
    }

    public void RaiseConfigChanged()
    {
        OnConfigChanged.Invoke();
    }

    public void ReloadConfig()
    {
        // 重新从数据库获取
        LoadFromDataBaseToCache();

        Interlocked.Exchange(ref this._changeToken, new ReverseProxyConfigChangeToken())
            .OnReload();
    }

    public IChangeToken GetReloadToken() => _changeToken;

    private void LoadFromDataBaseToCache()
    {
        var config = GetFromDataBase();
        SetToCache(config);
    }

    private GatewayProxyConfig GetFromDataBase()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GatewayDbContext>();

        var clusters = dbContext.ServiceClusters
            .Include(x => x.Destinations)
            .Include(x => x.HealthCheckConfig)
            .AsNoTracking()
            .ToList();
        var routes = dbContext.ServiceRoutes
            .Include(x => x.Match)
            .AsNoTracking()
            .ToList();

        return ConstructConfigFromEntity(clusters, routes);
    }

    private GatewayProxyConfig ConstructConfigFromEntity(List<ServiceCluster> clusters, List<ServiceRoute> routes)
    {
        var clusterConfigs = clusters.Select(cluster => cluster.ToYarpClusterConfig()).ToList();

        var routeConfigs = routes.Select(route => route.ToYarpRouteConfig()).ToList();

        return new(routeConfigs, clusterConfigs);
    }

    private GatewayProxyConfig? TryGetFromCache()
    {
        try
        {
            var bytes = _distributedCache.Get(CacheKey);
            if (bytes is not null)
            {
                return JsonSerializer.Deserialize<GatewayProxyConfig>(bytes);
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Deserialize ProxyConfig error");
        }

        return default;
    }

    private void SetToCache(GatewayProxyConfig config)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(config);
        _distributedCache.Set(CacheKey, bytes);
    }
}
