using Microsoft.EntityFrameworkCore;
using NetNet.Gateway.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NetNet.Gateway;

[ConnectionStringName("Default")]
public class GatewayDbContext : AbpDbContext<GatewayDbContext>
{
    public GatewayDbContext(DbContextOptions<GatewayDbContext> options) : base(options)
    {
    }

    public DbSet<Cluster> Clusters { get; set; }
    public DbSet<ProxyRoute> ProxyRoutes { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<ActiveHealthCheckOptions> ActiveHealthCheckOptions { get; set; }
    public DbSet<HealthCheckOptions> HealthCheckOptions { get; set; }
    public DbSet<Metadata> Metadatas { get; set; }
    public DbSet<PassiveHealthCheckOptions> PassiveHealthCheckOptions { get; set; }
    public DbSet<HttpClientConfig> ProxyHttpClientOptions { get; set; }
    public DbSet<ProxyMatch> ProxyMatches { get; set; }
    public DbSet<ForwarderRequest> RequestProxyOptions { get; set; }
    public DbSet<RouteHeader> RouteHeaders { get; set; }
    public DbSet<SessionAffinityConfig> SessionAffinityOptions { get; set; }
    public DbSet<SessionAffinityOptionSetting> SessionAffinityOptionSettings { get; set; }
    public DbSet<Transform> Transforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


    }
}
