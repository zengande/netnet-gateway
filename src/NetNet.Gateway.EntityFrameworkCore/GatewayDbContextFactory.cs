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
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new GatewayDbContext(builder.Options, NullLoggerFactory.Instance);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../NetNet.Gateway.Admin/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets("06a24603-e499-4408-8ad9-921653317675");

        return builder.Build();
    }
}
