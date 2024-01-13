using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace NetNet.Gateway;

public class GatewayDbContextFactory : IDesignTimeDbContextFactory<GatewayDbContext>
{
    public GatewayDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("Default");
        var builder = new DbContextOptionsBuilder<GatewayDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new GatewayDbContext(builder.Options, NullLoggerFactory.Instance);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NetNet.Gateway.Ingress/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets("bfb2502b-78b6-487d-bebc-20d9a472bfcb");

        return builder.Build();
    }
}
