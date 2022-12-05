using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureServiceClusterAggregate();
    }
}
