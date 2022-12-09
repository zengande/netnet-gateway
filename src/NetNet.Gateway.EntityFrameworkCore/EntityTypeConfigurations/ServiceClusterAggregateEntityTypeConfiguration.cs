using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
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
        ConfigureServiceHealthCheckConfig(modelBuilder.Entity<ServiceClusterHealthCheckConfig>());
    }

    private static void ConfigureServiceCluster(EntityTypeBuilder<ServiceCluster> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusters")
            .HasComment("服务集群");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.LoadBalancingPolicy)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("负载均衡策略");
        builder.OwnsOne(x => x.HttpRequestConfig, config =>
        {
            config.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusterHttpRequestConfigs")
                .HasComment("服务HTTP请求配置");
            config.WithOwner().HasForeignKey("ServiceClusterId");
            config.Property<Guid>("Id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            config.HasKey("Id");
            config.Property(x => x.Version)
                .IsRequired(false)
                .HasColumnName("HttpVersion")
                .HasConversion<VersionValueConverter>()
                .HasMaxLength(200)
                .HasDefaultValue(HttpVersion.Version20)
                .HasComment("Http版本");
            config.Property(x => x.VersionPolicy)
                .IsRequired(false)
                .HasColumnName("HttpVersionPolicy")
                .HasDefaultValue(HttpVersionPolicy.RequestVersionOrLower)
                .HasComment("Http版本策略");
            config.Property(x => x.ActivityTimeoutSeconds)
                .IsRequired(false)
                .HasComment("超时秒数");
            config.Property(x => x.AllowResponseBuffering)
                .IsRequired(false)
                .HasComment("是否允许相应缓冲");
        });
        builder.OwnsOne(x => x.HttpClientConfig, config =>
        {
            config.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusterHttpClientConfigs")
                .HasComment("服务HTTP客户端配置");
            config.WithOwner().HasForeignKey("ServiceClusterId");
            config.Property<Guid>("Id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            config.HasKey("Id");
            config.Property(x => x.SslProtocols)
                .IsRequired(false)
                .HasComment("TLS协议");
            config.Property(x => x.RequestHeaderEncoding)
                .IsRequired(false)
                .HasMaxLength(200)
                .HasComment("请求头编码");
            config.Property(x => x.MaxConnectionsPerServer)
                .IsRequired(false)
                .HasComment("最大连接数");
            config.Property(x => x.EnableMultipleHttp2Connections)
                .IsRequired(false)
                .HasComment("是否建立HTTP/2连接");
            config.Property(x => x.DangerousAcceptAnyServerCertificate)
                .IsRequired(false)
                .HasComment("是否忽略HTTPS证书错误");
        });

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_ServiceClusters_Name");

        builder.Metadata.FindNavigation(nameof(ServiceCluster.Destinations))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasOne(x => x.HealthCheckConfig);
    }

    private static void ConfigureServiceDestination(EntityTypeBuilder<ServiceDestination> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceDestinations")
            .HasComment("服务目的地");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("key");
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("地址");
        builder.Property(x => x.Health)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("健康检查地址");
        builder.Property(x => x.Metadata)
            .HasConversion<AbpJsonValueConverter<Dictionary<string, string>>>();
    }

    private static void ConfigureServiceHealthCheckConfig(EntityTypeBuilder<ServiceClusterHealthCheckConfig> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusterHealthCheckConfig");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.ServiceClusterId)
            .IsRequired();
        builder.Property(x => x.AvailableDestinationsPolicy)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("可用终点策略");

        builder.OwnsOne(x => x.Active);
        builder.OwnsOne(x => x.Passive);
    }
}
