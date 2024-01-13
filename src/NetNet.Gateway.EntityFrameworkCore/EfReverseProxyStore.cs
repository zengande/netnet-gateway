using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using System.Text.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace NetNet.Gateway;

public class EfReverseProxyStore : IReverseProxyStore, ISingletonDependency
{
    private const string CacheKey = "netnet:gateway:proxyconfig";

    public event ConfigChangedHandler OnConfigChanged;

    private ReverseProxyConfigChangeToken _changeToken = new();

    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<EfReverseProxyStore> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IClock _clock;

    public EfReverseProxyStore(IDistributedCache distributedCache, ILogger<EfReverseProxyStore> logger,
        IServiceProvider serviceProvider, IClock clock)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _clock = clock;

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
        _logger.LogInformation("【YARP】配置发生已改变：{0}", nameof(RaiseConfigChanged));

        OnConfigChanged.Invoke();
    }

    public void ReloadConfig()
    {
        _logger.LogInformation("【YARP】重新从数据加载配置");

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

        _logger.LogInformation("【YARP】配置重新加载完成，当前版本：{0}", config.Version);
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
        var version = $"v{_clock.Now:yyMMddHHmmssfff}";

        var clusterConfigs = clusters.Select(cluster => cluster.ToYarpClusterConfig()).ToList();

        var routeConfigs = routes.Select(route => route.ToYarpRouteConfig()).ToList();

        return new(version, routeConfigs, clusterConfigs);
    }

    private GatewayProxyConfig? TryGetFromCache()
    {
        try
        {
            var bytes = _distributedCache.Get(CacheKey);
            if (bytes is null) return default;
            return JsonSerializer.Deserialize<GatewayProxyConfig>(bytes);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "【YARP】反序列化错误");
        }

        return default;
    }

    private void SetToCache(GatewayProxyConfig config)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(config);
        _distributedCache.Set(CacheKey, bytes);
    }
}
