using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NetNet.Gateway.AggregateModels.ServiceRouteAggregate;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NetNet.Gateway.EntityTypeConfigurations;

internal static class ServiceRouteAggregateEntityTypeConfiguration
{
    internal static void ConfigureServiceRouteAggregate(this ModelBuilder modelBuilder)
    {
        ConfigureServiceRoute(modelBuilder.Entity<ServiceRoute>());
        ConfigureServiceRouteMatch(modelBuilder.Entity<ServiceRouteMatch>());
    }

    private static void ConfigureServiceRoute(EntityTypeBuilder<ServiceRoute> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "routes", x => x.HasComment("服务路由"));
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>()
            .HasColumnName("id");
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasColumnName("name")
            .HasComment("路由名称");
        builder.Property(x => x.ServiceClusterId)
            .IsRequired()
            .HasColumnName("service_cluster_id")
            .HasComment("服务id");
        builder.Property(x => x.AuthorizationPolicy)
            .IsRequired(false)
            .HasColumnName("authorization_policy")
            .HasMaxLength(200)
            .HasComment("授权策略");
        builder.Property(x => x.CrosPolicy)
            .IsRequired(false)
            .HasColumnName("cros_policy")
            .HasMaxLength(200)
            .HasComment("跨域策略");
        builder.Property(x => x.Order)
            .IsRequired(false)
            .HasColumnName("order")
            .HasComment("排序");
        builder.OwnsMany(x => x.Transforms, transform =>
        {
            transform.ToTable(GatewayEfConstant.TablePrefix + "route_transforms")
                .HasComment("服务路由请求转换配置");
            transform.WithOwner().HasForeignKey("service_route_id");
            transform.Property<Guid>("id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            transform.HasKey("id");
            transform.Property(x => x.GroupIndex)
                .IsRequired()
                .HasColumnName("group_index")
                .HasComment("分组索引");
            transform.Property(x => x.Key)
                .IsRequired()
                .HasColumnName("key")
                .HasMaxLength(200)
                .HasComment("Key");
            transform.Property(x => x.Value)
                .IsRequired()
                .HasColumnName("value")
                .HasMaxLength(500)
                .HasComment("Value");
        });
        builder.OwnsMany(x => x.Metadata, metadata =>
        {
            metadata.ToTable(GatewayEfConstant.TablePrefix + "ServiceRouteMetadata")
                .HasComment("服务路由元数据");
            metadata.WithOwner().HasForeignKey("ServiceRouteId");
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

        builder.HasOne(x => x.Match);
    }

    private static void ConfigureServiceRouteMatch(EntityTypeBuilder<ServiceRouteMatch> builder)
    {
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceRouteMatches")
            .HasComment("服务路由匹配规则");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.ServiceRouteId)
            .IsRequired();
        builder.Property(x => x.Hosts)
            .IsRequired(false)
            .HasMaxLength(2000)
            .HasComment("请求主机（逗号分隔）");
        builder.Property(x => x.Methods)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("请求方法（逗号分隔）");
        builder.Property(x => x.Path)
            .IsRequired(false)
            .HasMaxLength(500)
            .HasComment("请求路径");
    }
}
