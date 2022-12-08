using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using NetNet.Gateway.EntityTypeConfigurations;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NetNet.Gateway;

[ConnectionStringName("Default")]
public class GatewayDbContext : AbpDbContext<GatewayDbContext>
{
    private readonly ILoggerFactory _loggerFactory;
    public GatewayDbContext(DbContextOptions<GatewayDbContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    #region ServiceCluster

    public virtual DbSet<ServiceCluster> ServiceClusters { get; set; }
    public virtual DbSet<ServiceDestination> ServiceDestinations { get; set; }

    #endregion

    #region ServiceRoute

    public virtual DbSet<ServiceRoute> ServiceRoutes { get; set; }
    public virtual DbSet<ServiceRouteMatch> ServiceRouteMatches { get; set; }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureServiceClusterAggregate();
        modelBuilder.ConfigureServiceRouteAggregate();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
