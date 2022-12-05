using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetNet.Gateway.AggregateModels.ServiceClusterAggregate;
using NetNet.Gateway.ValueConversions;
using System.Net;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace NetNet.Gateway.EntityTypeConfigurations;

internal static class ServiceClusterAggregateEntityTypeConfiguration
{
    internal static void ConfigureServiceClusterAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureServiceCluster(modelBuilder.Entity<ServiceCluster>());
        ConfigureServiceDestination(modelBuilder.Entity<ServiceDestination>());
    }

    private static void ConfigureServiceCluster(EntityTypeBuilder<ServiceCluster> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusters")
            .HasComment("服务集群");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.Description)
            .IsRequired()
            .HasDefaultValue(string.Empty)
            .HasMaxLength(500)
            .HasComment("描述");
        builder.Property(x => x.LoadBalancingPolicy)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("负载均衡策略");
        builder.OwnsOne(x => x.HttpRequestConfig, config =>
        {
            config.Property(x => x.Version)
                .HasColumnName("HttpVersion")
                .HasConversion<VersionValueConverter>()
                .HasMaxLength(200)
                .HasDefaultValue(HttpVersion.Version20)
                .HasComment("Http版本");
            config.Property(x => x.VersionPolicy)
                .HasColumnName("HttpVersionPolicy")
                .HasDefaultValue(HttpVersionPolicy.RequestVersionOrLower)
                .HasComment("Http版本策略");
            config.Property(x => x.ActivityTimeoutSeconds)
                .HasComment("超时秒数");
            config.Property(x => x.AllowResponseBuffering)
                .HasComment("是否允许相应缓冲");
        });
        builder.OwnsOne(x => x.HttpClientConfig);

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_ServiceClusters_Name");

        builder.Metadata.FindNavigation(nameof(ServiceCluster.Destinations))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureServiceDestination(EntityTypeBuilder<ServiceDestination> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceDestinations")
            .HasComment("服务目的地");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Metadata)
            .HasConversion<AbpJsonValueConverter<Dictionary<string, string>>>();
    }
}
