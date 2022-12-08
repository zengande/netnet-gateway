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
        builder.ToTable(GatewayEfConstant.TablePrefix + "ServiceRoutes")
            .HasComment("服务路由");
        builder.ConfigureByConvention();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<SequentialGuidValueGenerator>();
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired()
            .HasComment("路由名称");
        builder.Property(x => x.ServiceClusterId)
            .IsRequired()
            .HasComment("服务id");
        builder.Property(x => x.AuthorizationPolicy)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("授权策略");
        builder.Property(x => x.CrosPolicy)
            .IsRequired(false)
            .HasMaxLength(200)
            .HasComment("跨域策略");
        builder.Property(x => x.Order)
            .IsRequired(false)
            .HasComment("排序");

        builder.OwnsMany(x => x.Transforms, transform =>
        {
            transform.ToTable(GatewayEfConstant.TablePrefix + "ServiceRouteTransforms")
                .HasAnnotation(RelationalAnnotationNames.Comment, "服务路由请求转换配置");
            transform.WithOwner().HasForeignKey("ServiceRouteId");
            transform.Property<Guid>("Id")
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd();
            transform.HasKey("Id");
            transform.Property(x => x.GroupIndex)
                .IsRequired()
                .HasComment("分组索引");
            transform.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Key");
            transform.Property(x => x.Value)
                .IsRequired()
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
