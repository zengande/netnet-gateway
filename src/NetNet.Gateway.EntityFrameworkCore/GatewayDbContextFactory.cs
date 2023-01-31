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

        var builder = new DbContextOptionsBuilder<GatewayDbContext>()
            .UseNpgsql(configuration.GetConnectionString("Default"));

        return new GatewayDbContext(builder.Options, NullLoggerFactory.Instance);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NetNet.Gateway.Admin/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets("28d2f441-4c8d-495f-91de-b2197aaf0914");

        return builder.Build();
    }
}
