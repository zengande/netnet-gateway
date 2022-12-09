﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetNet.Gateway;
using Volo.Abp.EntityFrameworkCore;

#nullable disable

namespace NetNet.Gateway.Migrations
{
    [DbContext(typeof(GatewayDbContext))]
    partial class GatewayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("_Abp_DatabaseProvider", EfCoreDatabaseProvider.SqlServer)
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceCluster", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("LoadBalancingPolicy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("负载均衡策略");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("名称");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("IX_ServiceClusters_Name");

                    b.ToTable("GW_ServiceClusters", (string)null);

                    b.HasComment("服务集群");
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterHealthCheckConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvailableDestinationsPolicy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("可用终点策略");

                    b.Property<Guid>("ServiceClusterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServiceClusterId")
                        .IsUnique();

                    b.ToTable("GW_ServiceClusterHealthCheckConfig", (string)null);
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceDestination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("地址");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<string>("Health")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("健康检查地址");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("key");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ServiceClusterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServiceClusterId");

                    b.ToTable("GW_ServiceDestinations", (string)null);

                    b.HasComment("服务目的地");
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRoute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthorizationPolicy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("授权策略");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("ConcurrencyStamp");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreationTime");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CreatorId");

                    b.Property<string>("CrosPolicy")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("跨域策略");

                    b.Property<string>("ExtraProperties")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ExtraProperties");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("LastModificationTime");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("LastModifierId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasComment("路由名称");

                    b.Property<int?>("Order")
                        .HasColumnType("int")
                        .HasComment("排序");

                    b.Property<Guid>("ServiceClusterId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("服务id");

                    b.HasKey("Id");

                    b.ToTable("GW_ServiceRoutes", (string)null);

                    b.HasComment("服务路由");
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRouteMatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Hosts")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasComment("请求主机（逗号分隔）");

                    b.Property<string>("Methods")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("请求方法（逗号分隔）");

                    b.Property<string>("Path")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("请求路径");

                    b.Property<Guid>("ServiceRouteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServiceRouteId")
                        .IsUnique();

                    b.ToTable("GW_ServiceRouteMatches", (string)null);

                    b.HasComment("服务路由匹配规则");
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceCluster", b =>
                {
                    b.OwnsOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterHttpClientConfig", "HttpClientConfig", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("DangerousAcceptAnyServerCertificate")
                                .HasColumnType("bit")
                                .HasComment("是否忽略HTTPS证书错误");

                            b1.Property<bool?>("EnableMultipleHttp2Connections")
                                .HasColumnType("bit")
                                .HasComment("是否建立HTTP/2连接");

                            b1.Property<int?>("MaxConnectionsPerServer")
                                .HasColumnType("int")
                                .HasComment("最大连接数");

                            b1.Property<string>("RequestHeaderEncoding")
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasComment("请求头编码");

                            b1.Property<Guid>("ServiceClusterId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int?>("SslProtocols")
                                .HasColumnType("int")
                                .HasComment("TLS协议");

                            b1.HasKey("Id");

                            b1.HasIndex("ServiceClusterId")
                                .IsUnique();

                            b1.ToTable("GW_ServiceClusterHttpClientConfigs", (string)null);

                            b1.HasComment("服务HTTP客户端配置");

                            b1.WithOwner()
                                .HasForeignKey("ServiceClusterId");
                        });

                    b.OwnsOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterHttpRequestConfig", "HttpRequestConfig", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int?>("ActivityTimeoutSeconds")
                                .HasColumnType("int")
                                .HasComment("超时秒数");

                            b1.Property<bool?>("AllowResponseBuffering")
                                .HasColumnType("bit")
                                .HasComment("是否允许相应缓冲");

                            b1.Property<Guid>("ServiceClusterId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Version")
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasDefaultValue("2.0")
                                .HasColumnName("HttpVersion")
                                .HasComment("Http版本");

                            b1.Property<int?>("VersionPolicy")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasDefaultValue(0)
                                .HasColumnName("HttpVersionPolicy")
                                .HasComment("Http版本策略");

                            b1.HasKey("Id");

                            b1.HasIndex("ServiceClusterId")
                                .IsUnique();

                            b1.ToTable("GW_ServiceClusterHttpRequestConfigs", (string)null);

                            b1.HasComment("服务HTTP请求配置");

                            b1.WithOwner()
                                .HasForeignKey("ServiceClusterId");
                        });

                    b.Navigation("HttpClientConfig")
                        .IsRequired();

                    b.Navigation("HttpRequestConfig")
                        .IsRequired();
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterHealthCheckConfig", b =>
                {
                    b.HasOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceCluster", null)
                        .WithOne("HealthCheckConfig")
                        .HasForeignKey("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterHealthCheckConfig", "ServiceClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterActiveHealthCheckConfig", "Active", b1 =>
                        {
                            b1.Property<Guid>("ServiceClusterHealthCheckConfigId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("Enabled")
                                .HasColumnType("bit");

                            b1.Property<int?>("IntervalSeconds")
                                .HasColumnType("int");

                            b1.Property<string>("Path")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Policy")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int?>("TimeoutSeconds")
                                .HasColumnType("int");

                            b1.HasKey("ServiceClusterHealthCheckConfigId");

                            b1.ToTable("GW_ServiceClusterHealthCheckConfig");

                            b1.WithOwner()
                                .HasForeignKey("ServiceClusterHealthCheckConfigId");
                        });

                    b.OwnsOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceClusterPassiveHealthCheckConfig", "Passive", b1 =>
                        {
                            b1.Property<Guid>("ServiceClusterHealthCheckConfigId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool?>("Enabled")
                                .HasColumnType("bit");

                            b1.Property<string>("Policy")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int?>("ReactivationPeriodSeconds")
                                .HasColumnType("int");

                            b1.HasKey("ServiceClusterHealthCheckConfigId");

                            b1.ToTable("GW_ServiceClusterHealthCheckConfig");

                            b1.WithOwner()
                                .HasForeignKey("ServiceClusterHealthCheckConfigId");
                        });

                    b.Navigation("Active")
                        .IsRequired();

                    b.Navigation("Passive")
                        .IsRequired();
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceDestination", b =>
                {
                    b.HasOne("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceCluster", null)
                        .WithMany("Destinations")
                        .HasForeignKey("ServiceClusterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRoute", b =>
                {
                    b.OwnsMany("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRouteTransform", "Transforms", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("GroupIndex")
                                .HasColumnType("int")
                                .HasComment("分组索引");

                            b1.Property<string>("Key")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasComment("Key");

                            b1.Property<Guid>("ServiceRouteId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("nvarchar(500)")
                                .HasComment("Value");

                            b1.HasKey("Id");

                            b1.HasIndex("ServiceRouteId");

                            b1.ToTable("GW_ServiceRouteTransforms", (string)null);

                            b1.HasComment("服务路由请求转换配置");

                            b1.WithOwner()
                                .HasForeignKey("ServiceRouteId");
                        });

                    b.Navigation("Transforms");
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRouteMatch", b =>
                {
                    b.HasOne("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRoute", null)
                        .WithOne("Match")
                        .HasForeignKey("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRouteMatch", "ServiceRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceClusterAggregate.ServiceCluster", b =>
                {
                    b.Navigation("Destinations");

                    b.Navigation("HealthCheckConfig")
                        .IsRequired();
                });

            modelBuilder.Entity("NetNet.Gateway.AggregateModels.ServiceRouteAggregate.ServiceRoute", b =>
                {
                    b.Navigation("Match")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
