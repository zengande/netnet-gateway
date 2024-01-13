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
        builder.ToTable(GatewayEfConstant.TablePrefix + "service_clusters")
            .HasComment("服务集群");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(200)
            .HasComment("名称");
        builder.Property(x => x.LoadBalancingPolicy)
            .IsRequired(false)
            .HasColumnName(("load_balancing_policy"))
            .HasMaxLength(200)
            .HasComment("负载均衡策略");
        builder.OwnsOne(x => x.HttpRequestConfig, config =>
        {
            config.ToTable(GatewayEfConstant.TablePrefix + "service_cluster_http_request_configs")
                .HasComment("服务HTTP请求配置");
            config.WithOwner()
                .HasForeignKey("service_cluster_id");
            config.Property<Guid>("id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            config.HasKey("id");
            config.Property(x => x.Version)
                .IsRequired(false)
                .HasColumnName("http_version")
                .HasConversion<VersionValueConverter>()
                .HasMaxLength(200)
                .HasDefaultValue(HttpVersion.Version20)
                .HasComment("Http版本");
            config.Property(x => x.VersionPolicy)
                .IsRequired(false)
                .HasColumnName("http_version_policy")
                .HasDefaultValue(HttpVersionPolicy.RequestVersionOrLower)
                .HasComment("Http版本策略");
            config.Property(x => x.ActivityTimeoutSeconds)
                .IsRequired(false)
                .HasColumnName("activity_timeout_seconds")
                .HasComment("超时秒数");
            config.Property(x => x.AllowResponseBuffering)
                .IsRequired(false)
                .HasColumnName("allow_response_buffering")
                .HasComment("是否允许相应缓冲");
        });
        builder.OwnsOne(x => x.HttpClientConfig, config =>
        {
            config.ToTable(GatewayEfConstant.TablePrefix + "service_cluster_http_client_configs")
                .HasComment("服务HTTP客户端配置");
            config.WithOwner().HasForeignKey("service_cluster_id");
            config.Property<Guid>("id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            config.HasKey("id");
            config.Property(x => x.SslProtocols)
                .IsRequired(false)
                .HasColumnName("ssl_protocols")
                .HasComment("TLS协议");
            config.Property(x => x.RequestHeaderEncoding)
                .IsRequired(false)
                .HasColumnName("request_header_encoding")
                .HasMaxLength(200)
                .HasComment("请求头编码");
            config.Property(x => x.MaxConnectionsPerServer)
                .IsRequired(false)
                .HasColumnName("max_connections_per_server")
                .HasComment("最大连接数");
            config.Property(x => x.EnableMultipleHttp2Connections)
                .IsRequired(false)
                .HasColumnName("enable_multiple_http2_connections")
                .HasComment("是否建立HTTP/2连接");
            config.Property(x => x.DangerousAcceptAnyServerCertificate)
                .IsRequired(false)
                .HasColumnName("dangerous_accept_any_server_certificate")
                .HasComment("是否忽略HTTPS证书错误");
        });
        builder.OwnsMany(x => x.Metadata, metadata =>
        {
            metadata.ToTable(GatewayEfConstant.TablePrefix + "ServiceClusterMetadata")
                .HasComment("服务元数据");
            metadata.WithOwner().HasForeignKey("ServiceClusterId");
            metadata.Property<Guid>("Id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            metadata.HasKey("Id");
            metadata.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Key");
            metadata.Property(x => x.Value)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasComment("Value");
        });

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Metadata.FindNavigation(nameof(ServiceCluster.Destinations))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasOne(x => x.HealthCheckConfig);
    }

    private static void ConfigureServiceDestination(EntityTypeBuilder<ServiceDestination> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "service_destinations")
            .HasComment("服务目的地");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .HasColumnName("id");
        builder.Property(x => x.Key)
            .IsRequired()
            .HasColumnName("key")
            .HasMaxLength(200)
            .HasComment("key");
        builder.Property(x => x.Address)
            .IsRequired()
            .HasColumnName("address")
            .HasMaxLength(500)
            .HasComment("地址");
        builder.Property(x => x.Health)
            .IsRequired(false)
            .HasColumnName("health")
            .HasMaxLength(500)
            .HasComment("健康检查地址");
        builder.Property(x => x.Metadata)
            .HasColumnName("metadata")
            .HasConversion<AbpJsonValueConverter<Dictionary<string, string>>>();
    }

    private static void ConfigureServiceHealthCheckConfig(EntityTypeBuilder<ServiceClusterHealthCheckConfig> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "service_cluster_health_checks");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .HasColumnName("id");
        builder.Property(x => x.ServiceClusterId)
            .IsRequired()
            .HasColumnName("service_cluster_id");
        builder.Property(x => x.AvailableDestinationsPolicy)
            .IsRequired(false)
            .HasColumnName(("available_destinations_policy"))
            .HasMaxLength(200)
            .HasComment("可用终点策略");

        builder.OwnsOne(x => x.Active);
        builder.OwnsOne(x => x.Passive);
    }
}
